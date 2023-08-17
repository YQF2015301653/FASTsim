using System;
using System.Collections;

namespace FASTsim.Library.SECS
{
    public class SECSTransaction
    {
        private short m_deviceid;
        internal bool m_inprogress;
        private SECSMessage m_primary;
        private SECSMessage m_secondary;
        private string m_description;
        private bool m_replyexpected;
        private bool m_autosys;
        private uint m_systembytes;
        private string m_tag;
        private static uint system;
        private uint m_stream;
        private uint m_function;
        private uint mTransactionId;
        private static uint mTransactionIdSeed;
        private static object m_Locker = new object();

        public SECSTransaction(uint stream, uint function)
        {
            if (((function & 1) != 1) && (function > 0))
            {
                function--;
            }
            m_primary = new SECSMessage("Primary", "");
            m_primary.Stream = stream;
            m_primary.Function = function;
            m_secondary = new SECSMessage("Secondary", "");
            m_secondary.Stream = stream;
            m_secondary.Function = ((function & 1) == 1) ? (function + 1) : function;
            m_inprogress = false;
            m_autosys = true;
            m_tag = "";
            m_stream = stream;
            m_function = function;
        }

        internal SECSTransaction(uint stream, uint function, uint system)
        {
            if (((function & 1) != 1) && (function > 0))
            {
                function--;
            }
            m_systembytes = system;
            m_primary = new SECSMessage("Primary", "");
            m_primary.Stream = stream;
            m_primary.Function = function;
            m_secondary = new SECSMessage("Secondary", "");
            m_secondary.Stream = stream;
            m_secondary.Function = ((function & 1) == 1) ? (function + 1) : function;
            m_inprogress = false;
            m_deviceid = 0;
            m_autosys = false;
            m_tag = "";
            m_stream = stream;
            m_function = function;
        }

        ~SECSTransaction()
        {
        }
        /// <summary>
        /// 复制当前的transaction
        /// </summary>
        /// <returns></returns>
        internal SECSTransaction Duplicate()
        {
            SECSTransaction transaction = new SECSTransaction(m_primary.Stream, m_primary.Function) {
                Description = m_description,
                ReplyExpected = m_replyexpected
            };
            transaction.Primary.Clonefrom(m_primary);
            transaction.Secondary.Clonefrom(m_secondary);
            m_deviceid = 0;
            return transaction;
        }

        private void HelpAssemble(ref byte[] d, ref int at, SECSItem s)
        {
            for (int i = 0; i < s.ItemCount; i++)
            {
                if (s.Item(i).Format != SECS_FORMAT.LIST)
                {
                    s.Item(i).Raw(ref d, ref at);
                }
                else
                {
                    s.Item(i).Raw(ref d, ref at);
                    HelpAssemble(ref d, ref at, s.Item(i));
                }
            }
        }

        private void HelpCodePage(int CodePage, SECSItem s)
        {
            for (int i = 0; i < s.ItemCount; i++)
            {
                if (s.Item(i).Format == SECS_FORMAT.LIST)
                {
                    HelpCodePage(CodePage, s.Item(i));
                }
                else if (s.Item(i).Format == SECS_FORMAT.ASCII)
                {
                    s.Item(i).m_CodePage = CodePage;
                }
            }
        }

        private void HelpEncodingCode(uint EncodingCode, SECSItem s)
        {
            for (int i = 0; i < s.ItemCount; i++)
            {
                if (s.Item(i).Format == SECS_FORMAT.LIST)
                {
                    HelpEncodingCode(EncodingCode, s.Item(i));
                }
                else if (s.Item(i).Format == SECS_FORMAT.CHAR2)
                {
                    s.Item(i).m_EncodingCode = EncodingCode;
                }
            }
        }

        internal void LoadList(Hashtable ht, ref int at, SECSItem parent, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SECSItem child = (SECSItem) ht[at];
                if (child.Format != SECS_FORMAT.LIST)
                {
                    at++;
                    parent.AddFast(child);
                }
                else
                {
                    parent.AddFast(child);
                    at++;
                    LoadList(ht, ref at, child, (int) child.hackcount);
                }
            }
        }

        internal void Parse(ref SECSMESSAGE s, ref byte[] src, WinSECS ws)
        {
            int num4;
            SECS_FORMAT secs_format;
            Hashtable ht = new Hashtable();
            int num = 0;
            int index = 0;
            SECSItem item = new SECSItem(null, null);
            int num6 = 0;
            int num7 = 0;
            item.Format = SECS_FORMAT.LIST;
            ht.Add(num++, item);
            goto TR_0019;
        TR_000A:
            if (ht.Count > 0)
            {
                int at = 1;
                ((SECSItem)ht[0]).hackcount = (uint)num7;
                LoadList(ht, ref at, (SECSItem)ht[0], (int)((SECSItem)ht[0]).hackcount);
                if ((s.Function & 1) > 0)
                {
                    m_primary.SetRoot((SECSItem)ht[0]);
                    return;
                }
                m_secondary.SetRoot((SECSItem)ht[0]);
            }
            return;
        TR_0019:
            while (true)
            {
                if (index >= src.Length)
                {
                    goto TR_000A;
                }
                else
                {
                    if (num6 == 0)
                    {
                        num7++;
                    }
                    else
                    {
                        num6--;
                    }
                    byte num5 = src[index++];
                    int num3 = num5 & 3;
                    secs_format = (SECS_FORMAT)(num5 >> 2);
                    if (num3 != 1)
                    {
                        if (num3 == 2)
                        {
                            if ((index + 1) < src.Length)
                            {
                                num4 = (src[index++] << 8) | src[index++];
                                break;
                            }
                        }
                        else
                        {
                            if (num3 != 3)
                            {
                                num4 = 0;
                                break;
                            }
                            if ((index + 1) < src.Length)
                            {
                                num4 = ((src[index++] << 0x10) | (src[index++] << 8)) | src[index++];
                                break;
                            }
                        }
                        goto TR_000A;
                    }
                    else if (index >= src.Length)
                    {
                        goto TR_000A;
                    }
                    else
                    {
                        num4 = src[index++];
                    }
                }
                break;
            }
            SECSItem item2 = new SECSItem(null, null);
            int length = src.Length;
            int num9 = index + num4;
            SECSItem.ParseItem(ws, ref item2, secs_format, ref src, index, num4);
            ht.Add(num++, item2);
            if (secs_format != SECS_FORMAT.LIST)
            {
                index += num4;
            }
            else
            {
                item2.hackcount = (uint)num4;
                num6 += num4;
            }
            goto TR_0019;
        }

        public static uint GetNextTransactionId()
        {
            uint ret = 0;

            lock (m_Locker)
            {
                if (mTransactionIdSeed == uint.MaxValue)
                {
                    mTransactionIdSeed = 1;
                }
                else
                {
                    mTransactionIdSeed += 1;
                }

                ret = mTransactionIdSeed;
            }

            return ret;
        }
        #region 收发数据

        public void Reply(WinSECS ws, uint tid)
        {
            HelpEncodingCode(ws.EncodingCode, m_secondary.Root);
            HelpCodePage(ws.CodePage, m_secondary.Root);
            byte[] d = new byte[m_secondary.Length];
            int at = 0;
            HelpAssemble(ref d, ref at, m_secondary.Root);
            if (m_deviceid == 0)
            {
                m_deviceid = (short)ws.Config.DeviceId;
            }
            SECSMESSAGE structmsg = new SECSMESSAGE
            {
                DeviceId = (uint)m_deviceid,
                Stream = m_secondary.Stream,
                Function = m_secondary.Function,
                IsReplyExpected = 0,
                System = SystemBytes,
                transactionId = tid
            };
            ws.SendMessage(structmsg, d, this);
        }

        public void Send(WinSECS ws)
        {
            HelpEncodingCode(ws.EncodingCode, m_primary.Root);
            HelpCodePage(ws.CodePage, m_primary.Root);
            byte[] d = new byte[m_primary.Length];
            int at = 0;
            HelpAssemble(ref d, ref at, m_primary.Root);
            SECSMESSAGE structmsg = new SECSMESSAGE();
            if (m_deviceid == 0)
            {
                m_deviceid = (short)ws.Config.DeviceId;
            }
            structmsg.DeviceId = (uint)m_deviceid;
            structmsg.Stream = m_primary.Stream;
            structmsg.Function = m_primary.Function;
            structmsg.IsReplyExpected = (uint)(m_replyexpected ? 1 : 0);
            structmsg.System = SystemBytes;
            ws.SendMessage(structmsg, d, this);
        }
        #endregion

        #region 属性值
        public virtual uint TransactionId
        {
            get
            {

                return this.mTransactionId;
            }
            set
            {
                mTransactionId = value;
            }
        }

        public string Name
        {
            get
            {
                //object[] objArray = new object[] { "S", m_primary.Stream, "F", m_primary.Function };
                //return string.Concat(objArray);
                object[] objArray = new object[] { "S", m_stream, "F", m_function };
                return string.Concat(objArray);
            }
            set
            {
                try
                {
                    uint stream;
                    uint function;
                    char[] s = new char[2] { 'S', 'F' };
                    string[] valueList = value.Split(s);
                    try
                    {
                        stream = uint.Parse(valueList[1]);
                        function = uint.Parse(valueList[2]);
                    }
                    catch (Exception)
                    {
                        throw new Exception("无效的Name");
                    }
                    if((function & 1) != 1)
                    {
                        throw new Exception("Function应为奇数");
                    }
                    m_stream = stream;
                    m_function = function;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public string Description
        {
            get =>
                m_description;
            set =>
                m_description = value;
        }

        public short DeviceID
        {
            get =>
                m_deviceid;
            set
            {
                if (value < 0)
                {
                    //throw new Exception(WinSECS.GetError("IDS_ERR_DEVICEIDINVALID"));
                }
                m_deviceid = value;
            }
        }

        public uint SystemBytes
        {
            get
            {
                if ((m_systembytes == 0) && m_autosys)
                {
                    if (++system == 0)
                    {
                        system++;
                    }
                    m_systembytes = system;
                }
                return m_systembytes;
            }
            set
            {
                if (m_autosys)
                {
                    throw new Exception("AutoSystemBytes prevented operation");
                }
                m_systembytes = value;
            }
        }

        public string Tag
        {
            get =>
                m_tag;
            set =>
                m_tag = value;
        }

        public bool ReplyExpected
        {
            get =>
                m_replyexpected;
            set =>
                m_replyexpected = value;
        }

        public bool AutoSystemBytes
        {
            get =>
                m_autosys;
            set =>
                m_autosys = value;
        }

        public SECSMessage Primary => m_primary;

        public SECSMessage Secondary => m_secondary;
        #endregion
    }
}

