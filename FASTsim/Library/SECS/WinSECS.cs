using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace FASTsim.Library.SECS
{
    public class WinSECS
    {
        public delegate void MessageEventHandler(SECSTransaction transaction, bool flag);
        public delegate void ErrorEventHandler(string errorMes);
        public event MessageEventHandler OnMessage;
        public event ErrorEventHandler OnMonitor;

        private int _iCodepage;
        private uint _uiEncodingCode;

        private object mLocker;
        private object mSendLocker;
        private object mT3Locker;

        private Dictionary<uint, SECSTransaction> mSecsTransaction; //远程主机transaction返回超时字典

        private SynchronizationContext mSyncContext;

        private System.Timers.Timer mTimer_T5;// Connect Separation Timeout
        private System.Timers.Timer mTimer_T6;// Control Transaction Timeout. 
        private System.Timers.Timer mTimer_T7;// NOT SELECTED Timeout
        private System.Timers.Timer mTimer_T8;// Network Intercharacter Timeout. 
        private System.Timers.Timer mTimer_Linktest;

        private Hashtable mT3Hash;
        private HSMSState mState;

        private Socket mSocketWorker;
        private Socket mSocketListener;
        private bool mKeepAlive;
        public List<TreeNode> sendMessageList;
        public ManualResetEvent sendState = new ManualResetEvent(false);

        private SECSLibrary m_library = null;

        private HSMSConfig mConfig;

        private int m_HsmsIsConnecting = 0;

        public HSMSConfig Config
        {
            get { return mConfig; }
            set { mConfig = value; }
        }


        public WinSECS()
        {
            sendMessageList = new List<TreeNode>();
            _iCodepage = -1;
            _uiEncodingCode = 1;

            mConfig = new HSMSConfig().Read() as HSMSConfig;

            mT3Hash = new Hashtable();
            mState = HSMSState.NOT_CONNECTED;
            mTimer_T5 = new System.Timers.Timer();
            mTimer_T5.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_T5_Elapsed);

            mTimer_T6 = new System.Timers.Timer();
            mTimer_T6.Interval = mConfig.T6Timeout * 1000;
            mTimer_T6.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_T6_Elapsed);

            mTimer_T7 = new System.Timers.Timer();
            mTimer_T7.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_T7_Elapsed);

            mTimer_T8 = new System.Timers.Timer();
            mTimer_T8.Interval = mConfig.T8Timeout * 1000;
            mTimer_T8.Elapsed += new System.Timers.ElapsedEventHandler(m_Timer_T8_Elapsed);

            mTimer_Linktest = new System.Timers.Timer();
            mTimer_Linktest.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_Linktest_Elapsed);

            mT3Locker = new object();
            mLocker = new object();
            mSendLocker = new object();

            mSecsTransaction = new Dictionary<uint, SECSTransaction>();

            mSyncContext = SynchronizationContext.Current;
            if (mSyncContext == null)
            {
                mSyncContext = new SynchronizationContext();
            }

        }

        #region OtherFunction
        public int CodePage
        {
            get => (_iCodepage != -1) ? _iCodepage : WsEncoding.DefaultCodePage();
            set
            {
                if (value == -1)
                {
                    _iCodepage = value;
                }
                else if (WsEncoding.IsValidCodePage(value))
                {
                    _iCodepage = value;
                }
            }
        }

        public uint EncodingCode
        {
            get =>
                _uiEncodingCode;
            set
            {
                if ((value != _uiEncodingCode) && ((value >= 1) && (value <= 14)))
                {
                    uint codePage = WsEncoding.GetCodePage((int)value);
                    if ((codePage != uint.MaxValue) && WsEncoding.IsValidCodePage((int)codePage))
                    {
                        _uiEncodingCode = value;
                    }
                }
            }
        }

        public void SetLibrary(SECSLibrary lbr)
        {
            m_library = lbr;
        }
        #endregion

        #region 注册事件
        private void PostEventTracedLog(string message)
        {
            if (OnMonitor != null)
            {
                mSyncContext.Post(delegate (object state)
                {
                    OnMonitor(message);
                }, null);
            }
        }

        private void PostEventErrorNotification(SECSException exceptionMes)
        {
            if (OnMonitor != null)
            {
                mSyncContext.Post(delegate (object state)
                {
                    OnMonitor(exceptionMes.ToString());
                }, null);
            }

        }

        private void ChangeHsmsState(HSMSState newState)
        {
            lock (mT3Locker)
            {
                if (mState != newState)
                {
                    mState = newState;
                }
            }
        }

        private void PostEventMessage(SECSTransaction transaction, bool flag)
        {
            if (OnMessage != null)
            {
                mSyncContext.Post(delegate (object state)
                {
                    OnMessage(transaction, flag);
                }, null);
            }
        }

        private void PostEventErrorNotification(string message)
        {
            if (OnMonitor != null)
            {
                mSyncContext.Post(delegate (object state)
                {
                    OnMonitor(message);
                }, null);
            }
        }
        #endregion

        #region Timers
        //T3 回复超时
        //        指发送指令到接收到回复指令的最大时间
        //T5 连接间隔
        //        指断开连接和重新连接的最小时间
        //T6 控制指令超时时间
        //        主要指连接选择，取消选择，连接检测等控制指令的回复最大时间
        //T7 连接超时
        //        指TCP/IP连接成功后到连接选择之间的最大时间，也就是发送stype=1 到收到stype=2 回复的这段时间
        //T8 接收超时
        //        指接收到的两个字符之间的最大时间
        private void T3TimerStart(uint tid)
        {
            lock (mT3Locker)
            {
                T3Timer timer_t3 = new T3Timer(tid);
                timer_t3.Interval = mConfig.T3Timeout * 1000;
                timer_t3.Elapsed += new System.Timers.ElapsedEventHandler(T3Timer_Elapsed);
                timer_t3.Start();
                mT3Hash.Add(tid, timer_t3);
            }
        }

        private void T3TimerStop(uint tid)
        {
            lock (mT3Locker)
            {
                if (mT3Hash.ContainsKey(tid))
                {
                    T3Timer timer_t3 = (T3Timer)mT3Hash[tid];
                    timer_t3.Stop();
                    mT3Hash.Remove(tid);
                }
            }
        }

        private void T3Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            T3Timer timer_t3 = (T3Timer)sender;
            timer_t3.Stop();

            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog(string.Format("T3 Timedout [TID:{0}, Interval:{1} sec]", timer_t3.TransactionId, mConfig.T3Timeout));
            }
            lock (mT3Locker)
            {
                if (mT3Hash.ContainsKey(timer_t3.TransactionId))
                {
                    mT3Hash.Remove(timer_t3.TransactionId);
                }
            }

            TransactionTimeout(timer_t3.TransactionId);

            timer_t3.Dispose();
        }

        private void T5_Timer_Start()
        {
            mTimer_T5.Interval = mConfig.T5Timeout * 1000;
            mTimer_T5.Start();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T5_Timer_Start");
            }
        }

        private void T5_Timer_Stop()
        {
            mTimer_T5.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T5_Timer_Stop");
            }
        }

        void mTimer_T5_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer_T5.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("m_Timer_T5_Elapsed");
            }

            //try to re-connect to ACTIVE
            //HsmsDriver_BeginConnect();
            if (mKeepAlive)
            {
                ReConnection();
            }
        }

        private void T6_Timer_Start()
        {
            mTimer_T6.Start();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T6_Timer_Start");
            }
        }

        private void T6_Timer_Stop()
        {
            mTimer_T6.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T6_Timer_Stop");
            }
        }

        private void mTimer_T6_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            T5_Timer_Start();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("m_Timer_T6_Elapsed");
            }
        }

        private void T7_Timer_Start()
        {
            mTimer_T7.Interval = mConfig.T7Timeout * 1000;
            mTimer_T7.Start();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T7_Timer_Start");
            }
        }

        private void T7_Timer_Stop()
        {
            mTimer_T7.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T7_Timer_Stop");
            }
        }

        private void mTimer_T7_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer_T7.Stop();
            ChangeHsmsState(HSMSState.NOT_CONNECTED);
            PassiveBeginAccept();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("m_Timer_T7_Elapsed");
            }
        }

        private void T8_Timer_Start()
        {
            mTimer_T8.Start();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T8_Timer_Start");
            }
        }

        private void T8_Timer_Stop()
        {
            mTimer_T8.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("T8_Timer_Stop");
            }
        }

        void m_Timer_T8_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer_T8.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("m_Timer_T8_Elapsed");
            }
            ChangeHsmsState(HSMSState.NOT_CONNECTED);
            PassiveBeginAccept();
        }

        private void LinkTest_Timer_Start()
        {
            if (mConfig.LinktestEnabled && mConfig.LinktestInterval > 0)
            {
                mTimer_Linktest.Interval = mConfig.LinktestInterval * 1000;
                mTimer_Linktest.Start();
                if (mConfig.ControlMessageLogEnable)
                {
                    PostEventTracedLog("LinkTest_Timer_Start");
                }
            }
        }

        private void LinkTest_Timer_Stop()
        {
            mTimer_Linktest.Stop();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("LinkTest_Timer_Stop");
            }
        }

        private void mTimer_Linktest_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer_Linktest.Stop();
            SendLinkTestRequest();
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("m_Timer_Linktest_Elapsed");
            }

        }

        #endregion

        #region S9 System Errors

        //private void SendS9F1(SECSTransaction timeoutTransaction)
        //{

        //    //if (timeoutTransaction == null)
        //    //{
        //    //    return;
        //    //}
        //    //ISECSItem item = new SECSItem(DataFormat.S2_B, timeoutTransaction.Message.Header);//MHEAD

        //    //SECSTransaction transaction = SECSTransaction.GetTransaction(9, 1);
        //    ////TODO TransactionId
        //    //transaction.TransactionId = SECSTransaction.GetNextTransactionId();
        //    //transaction.Message.Root = item;
        //    //SendMessage(timeoutTransaction);
        //}

        //private void SendS9F9(string message)
        //{

        //    if (message == "")
        //    {
        //        return;
        //    }
        //    SECSTransaction timeoutTransaction = m_library.Find(message);
        //}

        protected void TransactionTimeout(uint transactionId)
        {
            SECSTransaction secsTran;
            lock (mLocker)
            {
                if (mSecsTransaction.ContainsKey(transactionId))
                {
                    secsTran = mSecsTransaction[transactionId];
                    mSecsTransaction.Remove(transactionId);
                    sendState.Reset();
                    sendState.Set();
                    //TODO need send S9F9?
                    //if (HSMS_CONNECTION_MODE.PASSIVE == mConfig.Mode)
                    //{
                    //    SendS9F9(message);
                    //}
                }
                else
                {
                    //unknow transaction
                    PostEventErrorNotification("Transaction unknow Timeout,TID=" + transactionId);
                    return;
                }
            }

            if (secsTran != null)
            {
                PostEventErrorNotification(new SECSException("Transaction Timeout,TID=" + transactionId + ",TransactionName=" + secsTran.Name + "-" + secsTran.Description));
            }
            else
            {
                PostEventErrorNotification(new SECSException("Transaction Timeout,TID=" + transactionId));
            }
        }
        #endregion

        #region 连接过程
        public void Connect()
        {
            PostEventTracedLog("Start connect");

            try
            {
                T5_Timer_Stop();
                T6_Timer_Stop();
                T7_Timer_Stop();
                T8_Timer_Stop();
                LinkTest_Timer_Stop();

                mKeepAlive = true;

                if (mConfig.Mode == HSMS_CONNECTION_MODE.ACTIVE)
                {
                    PostEventTracedLog("Connect as ACTIVE mode");
                    //设备作为客户端
                    HsmsDriver_BeginConnect();
                    //start T5
                    T5_Timer_Start();
                }
                else
                {
                    PostEventTracedLog("Connect as PASSIVE mode at " + mConfig.IPAddress + ":" + mConfig.PortNo);

                    PassiveBeginAccept();
                }
            }
            catch (Exception ex)
            {
                if (mConfig.Mode == HSMS_CONNECTION_MODE.ACTIVE)
                {
                    Interlocked.Exchange(ref m_HsmsIsConnecting, 0);
                }
                else
                {
                    //Interlocked.Exchange(ref mHsmsAccepting, 0);
                }
                PostEventErrorNotification(new SECSException("Connect failed:" + ex.Message, ex));
            }
        }

        private void ReConnection()
        {
            PostEventTracedLog("start ReConnection");
            ChangeHsmsState(HSMSState.NOT_CONNECTED);

            T6_Timer_Stop();
            T7_Timer_Stop();
            T8_Timer_Stop();
            LinkTest_Timer_Stop();
            //try to connect
            if (mConfig.Mode == HSMS_CONNECTION_MODE.ACTIVE)
            {
                //after T5 interval retry
                T5_Timer_Start();
            }
            else
            {
                PassiveBeginAccept();
            }
        }

        public void Disconnect()
        {
            PostEventTracedLog("Disconnect");
            T5_Timer_Stop();
            T6_Timer_Stop();
            T7_Timer_Stop();
            T8_Timer_Stop();
            LinkTest_Timer_Stop();

            mKeepAlive = false;

            ChangeHsmsState(HSMSState.NOT_CONNECTED);

            if (mSocketWorker != null)
            {
                if (mSocketWorker.Connected)
                {
                    SendRequestControlMessage(SType.SeparateReq);
                }
                mSocketWorker.Close();
                mSocketWorker = null;
            }

            if (mSocketListener != null)
            {
                mSocketListener.Close();
                mSocketListener = null;
            }

        }
        #endregion

        #region PASSIVE

        private int mHsmsAccepting = 0;

        private void PassiveBeginAccept()
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("PassiveBeginAccept");
            }
            //Interlocked.Exchange以原子操作的形式，将 32 位有符号整数设置为指定的值并返回原始值。
            if (Interlocked.Exchange(ref mHsmsAccepting, 1) != 0)
            {
                return;
            }

            //close socket
            if (mSocketWorker != null && mSocketWorker.Connected)
            {
                mSocketWorker.Close();
            }

            //start listening if not listening
            if (mSocketListener == null || !mSocketListener.IsBound)
            {
                mSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //mSocketListener.Bind(new IPEndPoint(System.Net.IPAddress.Any, mConfig.PortNo));
                mSocketListener.Bind(new IPEndPoint(IPAddress.Parse(mConfig.IPAddress), mConfig.PortNo));
                mSocketListener.Listen(0);
            }

            try
            {
                //accept new socket connection
                mSocketListener.BeginAccept(HsmsDriver_BeginAcceptCallback, mSocketListener);
            }
            catch (Exception se)
            {
                //this port is already opened
                //if (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                //{
                //    throw se;
                //}
                throw se;
            }
            finally
            {
                //clear locking
                Interlocked.Exchange(ref mHsmsAccepting, 0);
            }
        }

        private void HsmsDriver_BeginAcceptCallback(IAsyncResult ar)
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("HsmsDriver_BeginAcceptCallback");
            }
            Interlocked.Exchange(ref mHsmsAccepting, 0);

            if (mKeepAlive)
            {
                try
                {
                    mSocketWorker = mSocketListener.EndAccept(ar);
                }
                catch (SocketException ex)
                {
                    ChangeHsmsState(HSMSState.NOT_CONNECTED);
                    PostEventErrorNotification(new SECSException("EndAccept failed:" + ex.Message, ex));
                    //TODO need reconnect??
                    if (mKeepAlive)
                    {
                        ReConnection();
                    }
                    return;
                }
                //hsms state > connect but not select
                ChangeHsmsState(HSMSState.NOT_SELECTED);
                //start T7,check select msg within T7 
                T7_Timer_Start();
                //wait Select.Req   
                WaitForMessageHeader(mSocketWorker);
            }
        }

        #endregion

        #region ACTIVE

        private void HsmsDriver_BeginConnect()
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("HsmsDriver_BeginConnect");
            }
            if (Interlocked.Exchange(ref m_HsmsIsConnecting, 1) != 0)
            {
                return;
            }

            IPAddress ip = IPAddress.Parse(mConfig.IPAddress);

            if (mSocketWorker != null && mSocketWorker.Connected)
            {
                mSocketWorker.Close();
            }
            mSocketWorker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            mSocketWorker.BeginConnect(new IPEndPoint(ip, mConfig.PortNo), HsmsDriver_BeginConnectCallback, mSocketWorker);

        }

        private void HsmsDriver_BeginConnectCallback(IAsyncResult ar)
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("HsmsDriver_BeginConnectCallback");
            }
            Interlocked.Exchange(ref m_HsmsIsConnecting, 0);

            Socket sck = (Socket)ar.AsyncState;

            if (mKeepAlive && sck.Connected)
            {
                sck.EndConnect(ar);
                //change state to NOT_SELECTED
                ChangeHsmsState(HSMSState.NOT_SELECTED);
                //stop T5
                T5_Timer_Stop();
                //wait message
                WaitForMessageHeader(sck);
                //send Select.Req
                SendSelectRequest();

            }
            else
            {
                //reset state to NOT_CONNECTED
                ChangeHsmsState(HSMSState.NOT_CONNECTED);
                //start T5 for retry
                T5_Timer_Start();
            }
        }

        #endregion

        #region 接收数据

        private void WaitForMessageHeader(Socket sck)
        {
            //header 固定长度=14
            byte[] header = new byte[14];
            WaitForMessageHeader(sck, header, 0);
        }

        private void WaitForMessageHeader(Socket sck, byte[] header, int headerStartIndex)
        {
            if (sck.Connected)
            {
                SocketStateHolder holder = new SocketStateHolder();
                holder.Socket = sck;
                holder.Header = header;
                holder.RecvStartIndex = headerStartIndex;

                SocketError se;

                try
                {
                    sck.BeginReceive(header, headerStartIndex, header.Length - headerStartIndex, SocketFlags.None, out se, WaitForMessageHeaderCallback, holder);
                }
                catch (Exception ex)
                {
                    PostEventErrorNotification(new SECSException("WaitForMessageHeader failed:" + ex.Message, ex));
                    if (mKeepAlive)
                    {
                        ReConnection();
                    }
                }

            }
        }

        private void WaitForMessageHeaderCallback(IAsyncResult ar)
        {
            SocketStateHolder holder = (SocketStateHolder)ar.AsyncState;

            Socket sck = holder.Socket;

            int byteCount = 0;

            try
            {
                if(sck.Connected != false)
                {
                    byteCount = sck.EndReceive(ar);
                }
            }
            catch (Exception ex)
            {
                PostEventErrorNotification(new SECSException("EndReceive failed:" + ex.Message, ex));
                if (mKeepAlive)
                {
                    ReConnection();
                }
                return;
            }

            if (byteCount == 0)
            {
                return;
            }

            byte[] header = holder.Header;
            holder.RecvStartIndex += byteCount;

            if (holder.RecvStartIndex == header.Length)
            {
                SType sType = (SType)header[9];

                bool needBody = false;
                uint bodyByteCount = 0;

                if (sType == SType.Message)
                {
                    //Message Length
                    byte[] lbb = new byte[4];
                    Array.Copy(header, 0, lbb, 0, 4);
                    Array.Reverse(lbb);
                    //calc body length
                    bodyByteCount = BitConverter.ToUInt32(lbb, 0) - 10; //exclude header
                    needBody = bodyByteCount > 0; //header
                }
                else
                {
                    if (mConfig.RawLogEnable)
                    {
                        PostEventTracedLog("[R] " + GetBitString(header));
                    }
                }

                if (needBody)
                {
                    //TODO T8 start ??
                    byte[] body = new byte[bodyByteCount];
                    T8_Timer_Start();
                    WaitForMessageBody(sck, header, body, 0);

                }
                else
                {
                    byte statusCode = header[7];

                    //case of completed message and control message
                    switch (sType)
                    {
                        case SType.Message:
                            ProcessSecsMessageBytes(header);
                            break;
                        case SType.SelectReq:
                            PostEventTracedLog("[R]=>Select.req");
                            if (mConfig.Mode == HSMS_CONNECTION_MODE.PASSIVE)
                            {
                                //stop T7
                                T7_Timer_Stop();
                                ChangeHsmsState(HSMSState.SELECTED);
                                SendSelectReponse(header);
                                //start link test timer
                                LinkTest_Timer_Start();
                            }
                            break;
                        case SType.SelectResp:
                            PostEventTracedLog("[R]=>Select.rsp,statusCode=" + statusCode);
                            switch (statusCode)
                            {
                                case 0:
                                    if (mConfig.Mode == HSMS_CONNECTION_MODE.ACTIVE)
                                    {
                                        //T6 timer
                                        T6_Timer_Stop();
                                        ChangeHsmsState(HSMSState.SELECTED);
                                        //start link test timer
                                        LinkTest_Timer_Start();
                                    }
                                    break;
                                case 1:
                                    PostEventErrorNotification("SelectResp Communication Already Active. statusCode=" + statusCode);
                                    break;
                                case 2:
                                    PostEventErrorNotification("SelectResp Connection Not Ready. statusCode=" + statusCode);
                                    break;
                                case 3:
                                    PostEventErrorNotification("SelectResp Connection Exhaust. statusCode=" + statusCode);
                                    break;
                                default:
                                    PostEventErrorNotification("SelectResp Connection Status Is Unknown.=" + statusCode);
                                    break;
                            }
                            break;

                        case SType.RejectReq:
                            //TODO
                            if (mConfig.Mode == HSMS_CONNECTION_MODE.ACTIVE)
                            {
                                //T6 timer
                                T6_Timer_Stop();
                            }
                            byte reasonCode = header[6];
                            PostEventTracedLog("[R]=>Reject.req,reasonCode=" + reasonCode);
                            break;
                        case SType.LinktestReq:
                            if (mConfig.ControlMessageLogEnable)
                            {
                                PostEventTracedLog("[R]=>Linktest.req");
                            }
                            SendLinkTestResponse(header);
                            break;
                        case SType.LinktestResp:
                            //T6 timer stop
                            T6_Timer_Stop();
                            if (mConfig.ControlMessageLogEnable)
                            {
                                PostEventTracedLog("[R]=>Linktest.rsp,statusCode=" + statusCode);
                            }
                            LinkTest_Timer_Start();
                            break;
                        case SType.SeparateReq:
                            if (mConfig.ControlMessageLogEnable)
                            {
                                PostEventTracedLog("[R]=>Separate.req");
                            }
                            if (mKeepAlive)
                            {
                                ReConnection();
                            }
                            break;
                        default:
                            //unknow
                            break;
                    }
                    //continue to next message
                    WaitForMessageHeader(sck);
                }
            }
            else
            {
                WaitForMessageHeader(sck, header, holder.RecvStartIndex);
            }

            holder.Socket = null;
            holder.Header = null;
            holder.Body = null;
        }

        private void WaitForMessageBody(Socket sck, byte[] header, byte[] body, int bodyStartIndex)
        {
            if (sck.Connected)
            {
                SocketError se;
                SocketStateHolder holder = new SocketStateHolder();
                holder.Socket = sck;
                holder.Header = header;
                holder.Body = body;
                holder.RecvStartIndex = bodyStartIndex;

                try
                {
                    sck.BeginReceive(body, bodyStartIndex, body.Length - bodyStartIndex, SocketFlags.None, out se, WaitForMessageBodyCallback, holder);
                }
                catch (Exception ex)
                {
                    PostEventErrorNotification(new SECSException("WaitForMessageBody failed", ex));
                    if (mKeepAlive)
                    {
                        ReConnection();
                    }
                }
            }
            else
            {
                PostEventTracedLog("when receive message body,Socket connected=false");
            }
        }

        private void WaitForMessageBodyCallback(IAsyncResult ar)
        {
            SocketStateHolder holder = (SocketStateHolder)ar.AsyncState;
            Socket sck = holder.Socket;

            int rcvByteCount;

            try
            {
                rcvByteCount = sck.EndReceive(ar);
            }
            catch (Exception se)
            {
                PostEventErrorNotification(new SECSException("EndReceive failed:" + se.Message, se));
                if (mKeepAlive)
                {
                    ReConnection();
                }
                return;
            }

            byte[] header = holder.Header;
            byte[] body = holder.Body;
            holder.RecvStartIndex += rcvByteCount;

            if (holder.RecvStartIndex == body.Length)
            {
                T8_Timer_Stop();
                byte[] data = new byte[header.Length + body.Length];
                Array.Copy(header, 0, data, 0, header.Length);
                Array.Copy(body, 0, data, header.Length, body.Length);
                //process secs message
                ProcessSecsMessageBytes(data);
                //continue to next message
                WaitForMessageHeader(holder.Socket);
            }
            else
            {
                WaitForMessageBody(holder.Socket, header, body, holder.RecvStartIndex);
            }

        }

        /// <summary>
        /// This function is called from inherited class
        /// </summary>
        /// <param name="data">SecsMessage in byte[]</param>
        ///
        private void ProcessSecsMessageBytes(byte[] data)
        {
            try
            {
                SECSMESSAGE msg = ToSecsMessage(data);
                byte[] destination = new byte[msg.Size];
                Array.Copy(data, 14, destination, 0, msg.Size);
                SECSTransaction tran = new SECSTransaction(msg.Stream, msg.Function, msg.Size);
                tran.TransactionId = msg.transactionId;
                tran.Parse(ref msg, ref destination, this);
                if (mConfig.DeviceId != msg.DeviceId)
                {
                    //TODO S9F1 Unrecognized Device ID
                    //SendS9F1(t);
                    PostEventErrorNotification("Unrecognized Device ID:" + msg.DeviceId);
                }


                bool isSecondary = (msg.Function & 0x01) == 0x00;
                //接收SECS返回的数据，停止T3定时器
                OnReceiving(msg.transactionId, isSecondary);

                if (!isSecondary)
                {
                    Decorate(tran, true);
                    PostEventMessage(tran, true);
                    if(msg.IsReplyExpected == 1)
                    {
                        tran.Reply(this, msg.transactionId);
                        PostEventMessage(tran, false);
                    }
                }
                else
                {                    
                    SECSTransaction secsTran = null;
                    lock (mLocker)
                    {
                        if (mSecsTransaction.ContainsKey(msg.transactionId))
                        {
                            //get primary message
                            secsTran = mSecsTransaction[msg.transactionId];
                        }
                    }

                    if (secsTran != null)
                    {
                        if (msg.Function == 0)
                        {
                            //primary transaction was abort
                            PostEventErrorNotification(new SECSException("Transaction Aborted" + tran.Name));
                        }
                        else if (msg.Stream != 0)//Stream 0 is always defined as not used
                        {
                            Decorate(tran, false);
                            //unregister transaction
                            mSecsTransaction.Remove(msg.transactionId);
                            PostEventMessage(tran, false);
                            sendState.Reset();
                            sendState.Set();
                        }
                    }
                    else
                    {
                        PostEventErrorNotification(new SECSException("Unknow Transaction" + tran.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                PostEventErrorNotification(new SECSException("Parse SECSMessage error:" + ex.Message, ex));

                return;
            }
        }

        #endregion

        #region 处理接收的数据

        private string GetBitString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", " ");
        }

        public SECSMESSAGE ToSecsMessage(byte[] data)
        {
            SECSMESSAGE msg = new SECSMESSAGE();
            using (MemoryStream reader = new MemoryStream(data))
            {
                reader.Position = 0;

                byte[] lengthBytes = new byte[4];

                //get length byte
                reader.Read(lengthBytes, 0, lengthBytes.Length);
                Array.Reverse(lengthBytes);

                int dataLength = BitConverter.ToInt32(lengthBytes, 0);
                if (data.Length != dataLength + 4)
                {
                    //invalid data length
                    throw new Exception("Invalid data length");
                }

                //get header
                byte[] header = new byte[10];
                reader.Read(header, 0, header.Length);

                //get device id from header
                byte[] deviceIdBytes = new byte[2];
                Array.Copy(header, 0, deviceIdBytes, 0, 2);
                Array.Reverse(deviceIdBytes);
                ushort deviceId = BitConverter.ToUInt16(deviceIdBytes, 0);

                //get stream
                //S 7-bit unsigned integer
                byte stream = (byte)(header[2] & 0x7F);
                byte function = header[3];

                bool needReply = (header[2] & 0x80) == 0x80;

                //transactionId.
                byte[] transactionIdBytes = new byte[4];
                Array.Copy(header, 6, transactionIdBytes, 0, transactionIdBytes.Length);
                Array.Reverse(transactionIdBytes);
                uint transactionId = BitConverter.ToUInt32(transactionIdBytes, 0);

                msg.IsReplyExpected = (uint)(needReply ? 1 : 0);
                msg.Stream = stream;
                msg.DeviceId = deviceId;
                msg.Function = function;
                msg.Size = (uint)data.Length - 14;
                msg.transactionId = transactionId;
                return msg;
            }
        }

        internal bool Same(SECSItem dst, SECSItem src) => ((src.Format == SECS_FORMAT.LIST) || (dst.Format == SECS_FORMAT.LIST)) ? ((src.Format == SECS_FORMAT.LIST) && (dst.Format == SECS_FORMAT.LIST)) : true;

        internal bool Ldecorate(SECSItem dst, SECSItem src)
        {
            int nth;
            int num2;
            int itemCount;
            int num4;
            dst.Name = src.Name;
            dst.Description = src.Description;
            itemCount = src.ItemCount;
            num4 = dst.ItemCount;
            if (itemCount == 0)
            {
                return false;
            }
            num2 = 0;
            nth = 0;
            while (num2 < num4)
            {
                SECSItem item2 = dst.Item(num2);
                SECSItem item = src.Item(nth);
                if (Same(item2, item))
                {
                    if (item.Format == SECS_FORMAT.LIST)
                    {
                        Ldecorate(item2, item);
                    }
                    else
                    {
                        item2.Name = item.Name;
                        item2.Description = item.Description;
                    }
                    if ((nth + 1) < itemCount)
                    {
                        nth++;
                    }
                }
                else if (item.Format != SECS_FORMAT.LIST)
                {
                    if (item2.Format == SECS_FORMAT.LIST)
                    {
                        if ((nth + 1) < itemCount)
                        {
                            nth++;
                        }
                        item2.Name = item.Name;
                        item2.Description = item.Description;
                    }
                }
                else
                {
                    if (nth > 0)
                    {
                        nth--;
                        item = src.Item(nth);
                    }
                    item2.Name = item.Name;
                    item2.Description = item.Description;
                    if ((nth + 1) < itemCount)
                    {
                        nth++;
                    }
                }
                num2++;
            }
            return true;
        }

        internal bool Decorate(SECSTransaction t, bool IsPrimary)
        {
            try
            {
                SECSTransaction transaction = null;
                try
                {
                    //transaction = m_library.Find(string.Concat(new object[] { "S", t.Primary.Stream, "F", t.Primary.Function}));
                    if (IsPrimary)
                    {
                        IDictionaryEnumerator enumTran = m_library.m_list.GetEnumerator();
                        while (enumTran.MoveNext())
                        {
                            if (enumTran.Key.ToString().Contains(t.Name))
                            {
                                transaction = (SECSTransaction)enumTran.Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        transaction = mSecsTransaction[t.TransactionId];
                    }
                }
                catch
                {
                }
                if (transaction != null)
                {
                    t.Description = transaction.Description;
                    t.Name = transaction.Name;
                    if (!IsPrimary)
                    {
                        try
                        {
                            Ldecorate(t.Secondary.Root, transaction.Secondary.Root);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            Ldecorate(t.Primary.Root, transaction.Primary.Root);
                        }
                        catch
                        {
                        }
                        t.Secondary.Clonefrom(transaction.Secondary);
                    }
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        #endregion

        #region Send message
        internal void SendMessage(SECSMESSAGE structmsg, byte[] data, SECSTransaction secsTran)
        {
            bool isPrimary = ((structmsg.IsReplyExpected & 0x01) == 0x01) && ((structmsg.Function & 0x01) == 0x01);

            if (isPrimary)
            {
                //get new transaction id
                structmsg.transactionId = SECSTransaction.GetNextTransactionId();
                //注册到超时字典中
                lock (mLocker)
                {
                    if (mSecsTransaction.ContainsKey(structmsg.transactionId))
                    {
                        mSecsTransaction.Remove(structmsg.transactionId);
                    }
                    mSecsTransaction.Add(structmsg.transactionId, secsTran);
                }
            }
            byte[] sendData = GetBytes(structmsg, data);
            //注册到T3中
            OnSending(structmsg.transactionId, isPrimary);
            if (mSocketWorker == null)
            {
                PostEventErrorNotification("未与远程主机建立连接");
                sendState.Reset();
                sendState.Set();
                return;
            }
            ProtectedSend(sendData);
        }

        public bool ProtectedSend(byte[] data)
        {
            SendBytes(data);
            return true;
        }

        private void SendBytes(byte[] data)
        {
            if (mSocketWorker.Connected)
            {
                SocketError se = SocketError.Success;
                lock (mSendLocker)
                {
                    mSocketWorker.BeginSend(data, 0, data.Length, SocketFlags.None, out se, SendBytesCallback, mSocketWorker);
                }

                if (mConfig.RawLogEnable)
                {
                    PostEventTracedLog("[S] " + GetBitString(data));
                }
            }
        }

        /// <summary>
        /// 发送失败回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SendBytesCallback(IAsyncResult ar)
        {
            Socket sck = (Socket)ar.AsyncState;

            if (sck.Connected)
            {
                try
                {
                    int sentByteCount = sck.EndSend(ar);
                }
                catch (Exception ex)
                {
                    PostEventErrorNotification(String.Format("{0}: SendBytesCallback Failed was due to Exception := {1}", mConfig.Mode, ex.Message));
                }

            }
            else
            {
                PostEventErrorNotification(String.Format("{0}: SendBytes Failed was due to Socket.Connected = {1}", mConfig.Mode, sck.Connected));
            }
        }

        protected void OnSending(uint tid, bool isPrimary)
        {
            if (mState == HSMSState.SELECTED && isPrimary)
            {
                //make sure the primary message will be register before
                //equipment will response secondary message
                T3TimerStart(tid);
            }
        }

        protected void OnReceiving(uint tid, bool isSecondary)
        {
            if (isSecondary)
            {
                T3TimerStop(tid);
            }
        }

        public byte[] GetBytes(SECSMESSAGE message, byte[] tmp1)
        {

            byte[] headerBytes = GetHeader(message);

            List<byte[]> itemArray = new List<byte[]>();

            int itemByteCount = 0;

            itemArray.Add(tmp1);
            itemByteCount += tmp1.Length;

            byte[] lengthBytes = BitConverter.GetBytes(headerBytes.Length + itemByteCount); //4-byte integer

            Array.Reverse(lengthBytes);

            byte[] allBytes = new byte[lengthBytes.Length + headerBytes.Length + itemByteCount];

            int index = 0;

            Array.Copy(lengthBytes, 0, allBytes, 0, lengthBytes.Length);
            index += lengthBytes.Length;

            Array.Copy(headerBytes, 0, allBytes, lengthBytes.Length, headerBytes.Length);
            index += headerBytes.Length;

            byte[] tmp2;
            for (int i = 0; i < itemArray.Count; i++)
            {
                tmp2 = itemArray[i];
                Array.Copy(tmp2, 0, allBytes, index, tmp2.Length);
                index += tmp2.Length;
            }

            itemArray.Clear();

            return allBytes;
        }

        public byte[] GetHeader(SECSMESSAGE message)
        {
            byte[] headerBytes = new byte[10];
            //device id
            byte[] deviceIdBytes = BitConverter.GetBytes(message.DeviceId);
            Array.Reverse(deviceIdBytes);

            headerBytes[0] = deviceIdBytes[0];
            headerBytes[1] = deviceIdBytes[1];
            //stream and w-bit
            /*
            1.A Primary Message which expects a Reply should set the W - Bit to 1.
            2.A Primary Message which does not expect a Reply should set the W-Bit to 0.
            3.A Reply Message should always set the W - Bit to 0.
            */
            if ((message.Function & 0x01) == 1 && (message.IsReplyExpected == 1) )
            {
                headerBytes[2] = (byte)(message.Stream | 0x80);
            }
            else
            {
                headerBytes[2] = (byte)(message.Stream & 0x7F);
            }
            //function
            //The Function is an 8-bit unsigned integer
            headerBytes[3] = (byte)message.Function;
            //ptype
            headerBytes[4] = 0;
            //stype
            headerBytes[5] = 0; //0 - mean to "SescSessionType.Message"

            //transaction id
            //byte[] transactionIdBytes = BitConverter.GetBytes(message.TransactionId);
            byte[] transactionIdBytes = BitConverter.GetBytes(message.transactionId);
            Array.Reverse(transactionIdBytes);

            headerBytes[6] = transactionIdBytes[0];
            headerBytes[7] = transactionIdBytes[1];
            headerBytes[8] = transactionIdBytes[2];
            headerBytes[9] = transactionIdBytes[3];

            return headerBytes;
        }


        #endregion

        #region Control Message

        private void SendLinkTestRequest()
        {
            if (mState != HSMSState.SELECTED)
            {
                PostEventErrorNotification("HSMS State is not in SELECTED state,can not send Linktest.req.");
                return;
            }
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("[S]=>Linktest.req");
            }
            T6_Timer_Start();
            SendRequestControlMessage(SType.LinktestReq);
        }

        private void SendLinkTestResponse(byte[] data)
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("[S]=>Linktest.rsp");
            }
            SendResponseControlMessage(data);
        }

        /// <summary>
        /// The Select Procedure shall be initiated only by the entity establishing the TCP/IP connection in active mode
        /// </summary>
        private void SendSelectRequest()
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("[S]=>Select.req");
            }
            T6_Timer_Start();
            SendRequestControlMessage(SType.SelectReq);
        }

        private void SendSelectReponse(byte[] data)
        {
            if (mConfig.ControlMessageLogEnable)
            {
                PostEventTracedLog("[S]=>Select.rsp");
            }
            SendResponseControlMessage(data);
        }

        private byte[] CreateControlMessage(SType sType)
        {
            //In HSMS-SS Control Messages, Session ID will always
            //assume the special value 0xFFFF(all one bits)
            return new byte[] { 0x00, 0x00, 0x00, 0x0A,
                0xFF, 0xFF, //device
                0x00, 0x00, //stream , function
                0x00, //pType
                (byte)sType, //sType
                0x00, 0x00, 0x00, 0x00  //transaction 
            };
        }

        private void SendRequestControlMessage(SType sType)
        {
            byte[] msg = CreateControlMessage(sType);

            byte[] tranId = BitConverter.GetBytes(SECSTransaction.GetNextTransactionId());
            Array.Reverse(tranId);
            Array.Copy(tranId, 0, msg, 10, 4);
            SendBytes(msg);
        }

        private void SendResponseControlMessage(byte[] data)
        {
            byte[] sendBuff = new byte[data.Length];
            Array.Copy(data, sendBuff, data.Length);
            sendBuff[9] = (byte)(sendBuff[9] + 1);
            SendBytes(sendBuff);
        }

        #endregion

        #region Private class

        private class SocketStateHolder
        {
            public Socket Socket;
            public byte[] Header;
            public byte[] Body;
            public int RecvStartIndex;
            public AutoResetEvent WaitEvent;
        }

        #endregion       
    }
}

