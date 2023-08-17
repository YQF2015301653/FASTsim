using FASTsim.Library.SECS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static FASTsim.Library.SECS.CommonData;

namespace FASTsim.GUI
{
    public partial class FASTsim : Form
    {
        private static TreeNode selectNode;
        private static SECSLibrary secsLibrary = new SECSLibrary("", "");
        private WinSECS winSecs;
        private PropertyForm propertyForm;
        private CommonControl selectCtl;
        private uint functionNum;
        private List<TreeNode> sendMessageList;
        //private List<TreeNode> sendMessageList = new List<TreeNode>();

        public event EventHandler SendMsgToPropertyEvent;

        public FASTsim()
        {
            InitializeComponent();
            InitSkin();
            InitTreeList();
            winSecs = new WinSECS();
            sendMessageList = winSecs.sendMessageList;
            winSecs.OnMessage += AddReceiveMessage;
            winSecs.OnMonitor += AddErrorMessage;
            winSecs.SetLibrary(secsLibrary);
        }

        #region PrivateFunction
        private void InitSkin()
        {
            openToolStripMenuItem1.Enabled = true;
            closeToolStripMenuItem.Enabled = false;
        }

        private void InitTreeList()
        {
            GetNodeTree(GetDefaultFilePath());
        }

        private TreeNode NewSECSMessage(SECSMessage message, string messageDes)
        {
            TreeNode mNode = GetNewNode(messageDes, message);
            GetChildItem(message.Root, mNode.Nodes);
            return mNode;
        }

        private void GetNodeTree(string filePath)
        {
            libraryTreeView.Nodes.Clear();
            string xmlFilePath = filePath;
            secsLibrary = new SECSLibrary("", "");
            secsLibrary.Load(xmlFilePath);
            TreeNode lNode = GetNewNode(secsLibrary.Description, secsLibrary);
            XmlToTree(lNode.Nodes);
            libraryTreeView.Nodes.Add(lNode);
        }

        private void XmlToTree(TreeNodeCollection nodes)
        {
            if (secsLibrary.m_list.m_array == null)
            {
                return;
            }
            foreach (object skey in secsLibrary.m_list.m_array)
            {
                SECSTransaction xmlNode = (SECSTransaction)secsLibrary.m_list.m_hash[skey];
                TreeNode tNode;
                if (xmlNode.Description == "")
                {
                    tNode = GetNewNode(xmlNode.Name, xmlNode);
                }
                else
                {
                    tNode = GetNewNode(xmlNode.Name + " - " + xmlNode.Description, xmlNode);
                }
                nodes.Add(tNode);
                if(xmlNode.Primary.Name == "")
                {
                    xmlNode.Primary.Name = "Primary";
                }
                if (xmlNode.Primary.Description == "")
                {
                    xmlNode.Primary.Description = xmlNode.Description + " " +"request";
                }
                TreeNode pNode = NewSECSMessage(xmlNode.Primary, "P " + xmlNode.Primary.Name  + " - " + xmlNode.Primary.Description);
                tNode.Nodes.Add(pNode);

                if (xmlNode.Secondary.Name == "")
                {
                    xmlNode.Secondary.Name = "Secondary";
                }
                if (xmlNode.Secondary.Description == "")
                {
                    xmlNode.Secondary.Description = xmlNode.Description + " " + "acknowledge";
                }
                TreeNode sNode = NewSECSMessage(xmlNode.Secondary, "S " + xmlNode.Secondary.Name + " - " + xmlNode.Secondary.Description);
                tNode.Nodes.Add(sNode);
            }

        }

        private string ValueFormat(SECSItem node)
        {
            string format = node.Format.ToString();
            if (node.Format == SECS_FORMAT.ASCII)
            {
                format = "A";
            }
            else if (node.Format == SECS_FORMAT.BINARY)
            {
                format = "B";
            }
            else if (node.Format == SECS_FORMAT.JIS8)
            {
                format = "J";
            }
            return format;
        }

        private string GetLNodeText(SECSItem node)
        {
            string text;
            if (node.Name == "" && node.Description == "" || node.Name == null && node.Description == null)
            {
                text = "L";
            }
            else if (node.Description == "")
            {
                text = node.Name;
            }
            else
            {
                text = node.Name + " - " + node.Description;
            }
            return text;
        }

        private string GetNNodeText(SECSItem node)
        {
            string format = ValueFormat(node);
            string text;
            if (node.Value == null)
            {
                if (node.Description == "" || node.Description == null)
                {
                    text = format + " " + node.Name + " = " + "<nothing>";
                }
                else
                {
                    text = format + " " + node.Name + " = " + "<nothing>" + " - " + node.Description;
                }
            }
            else
            {              
                if (node.Description == "" || node.Description == null)
                {
                    if(format == "A" || format == "J")
                    {
                        text = format + " " + node.Name + " = " + "'" + node.Value.ToString() + "'";
                    }
                    else
                    {
                        text = format + " " + node.Name + " = " + node.Value.ToString();
                    }                  
                }
                else
                {
                    if (format == "A" || format == "J")
                    {
                        text = format + " " + node.Name + " = " + "'" + node.Value.ToString() + "'" + " - " + node.Description;
                    }
                    else
                    {
                        text = format + " " + node.Name + " = " + node.Value.ToString() + " - " + node.Description;
                    }                    
                }
            }
            return text;
        }

        private void GetChildItem(SECSItem itemNode, TreeNodeCollection nodes)
        {
            string Text;
            for (int i = 0; i < itemNode.ItemCount; i++)
            {
                SECSItem node = itemNode.Item(i);
                if (node.Format == SECS_FORMAT.LIST)
                {
                    Text = GetLNodeText(node);
                }
                else
                {
                    Text = GetNNodeText(node);
                }
                TreeNode treeNode = GetNewNode(Text, node);
                nodes.Add(treeNode);
                if (node.ItemCount > 0)
                {
                    GetChildItem(node, treeNode.Nodes);
                }
            }
        }

        private string GetTranName()
        {
            string tranName = "";
            string[] strList = selectNode.Text.Split(new char[] { '-' }, 2);
            for(int i=0; i< strList.Length; i++)
            {
                tranName += strList[i].Trim();
            }
            return tranName;
        }

        private TreeNode GetNewNode(string text, object tag = null)
        {
            TreeNode treeNode = new TreeNode();
            treeNode.Text = text;
            treeNode.Tag = tag;
            return treeNode;
        }

        private string GetDefaultFilePath()
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string xmlFilePath = Path.GetDirectoryName(location) + "\\default.xml";
            return xmlFilePath;
        }

        private void SaveFile(string filter, string directoryPath = "Desktop")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = directoryPath;
            saveFileDialog.Filter = filter;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                secsLibrary.Save(filePath);
            }
        }

        private void OpenFile(string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.ShowDialog();
            string filePath = ofd.FileName;
            GetNodeTree(filePath);
        }

        private void DeleteCtlEvent()
        {
            if (selectCtl != null)
            {
                SendMsgToPropertyEvent -= selectCtl.MainFormNodeChaned;
            }
        }

        private void AddCtlEvent()
        {
            int level = selectNode.Level;
            switch (level)
            {
                case 0:
                    selectCtl = (CommonControl)propertyForm.mCtrl[0];
                    SendMsgToPropertyEvent += selectCtl.MainFormNodeChaned;
                    SendMsgToPropertyEvent(this, new MyEventArg() { Value = selectNode.Text });
                    break;
                case 1:
                    selectCtl = (CommonControl)propertyForm.mCtrl[1];
                    SendMsgToPropertyEvent += selectCtl.MainFormNodeChaned;
                    SendMsgToPropertyEvent(this, new MyEventArg() { Value = selectNode.Tag });
                    break;
                case 2:
                    selectCtl = (CommonControl)propertyForm.mCtrl[2];
                    SendMsgToPropertyEvent += selectCtl.MainFormNodeChaned;
                    SendMsgToPropertyEvent(this, new MyEventArg() { Value = selectNode.Tag });
                    break;
                default:
                    selectCtl = (CommonControl)propertyForm.mCtrl[3];
                    SendMsgToPropertyEvent += selectCtl.MainFormNodeChaned;
                    SendMsgToPropertyEvent(this, new MyEventArg() { Value = selectNode.Tag });
                    break;
            }
        }

        private void PropertyForm()
        {
            if (propertyForm == null)
            {
                propertyForm = new PropertyForm();
            }
            else
            {
                propertyForm.Dispose();
                propertyForm = new PropertyForm();
            }
        }

        #endregion

        #region Event
        static private void SaveAndShowLevel1(MyEventArg arg)
        {
            Dictionary<string, string> valueDic = (Dictionary<string, string>)arg.Value;
            SECSTransaction selectTran = (SECSTransaction)selectNode.Tag;
            SECSTransaction oldSecsTran = secsLibrary.Find(selectTran.Name + selectTran.Description);
            if (oldSecsTran == null)
            {
                throw new Exception("未查找到要编辑的transaction");
            }
            string oldName = oldSecsTran.Name;
            string oldDesc = oldSecsTran.Description;
            string oldTranKey = oldSecsTran.Name + oldSecsTran.Description;
            //SECSTransaction secsTran = oldSecsTran.Duplicate();
            try
            {
                if (valueDic["name"] == oldName)
                {
                    if (oldDesc == valueDic["desc"])
                    {
                        return;
                    }
                    if (secsLibrary.Find(oldName + valueDic["desc"]) == null)
                    {
                        oldSecsTran.Description = valueDic["desc"];
                        secsLibrary.Insert(oldSecsTran, selectNode.Index);
                        secsLibrary.Remove(oldTranKey);

                        char[] s = new char[2] { 'S', 'F' };
                        string[] valueList = valueDic["name"].Split(s);
                        if (valueList == null)
                        {
                            return;
                        }
                        oldSecsTran.Primary.Stream = uint.Parse(valueList[1]);
                        oldSecsTran.Primary.Function = uint.Parse(valueList[2]);
                        oldSecsTran.Secondary.Stream = uint.Parse(valueList[1]);
                        oldSecsTran.Secondary.Function = uint.Parse(valueList[2]) + 1;
                    }
                }
                else
                {
                    oldSecsTran.Name = valueDic["name"];
                    oldSecsTran.Description = valueDic["desc"];
                    secsLibrary.Insert(oldSecsTran, selectNode.Index);
                    secsLibrary.Remove(oldTranKey);

                    char[] s = new char[2] { 'S', 'F' };
                    string[] valueList = valueDic["name"].Split(s);
                    if (valueList == null)
                    {
                        return;
                    }
                    oldSecsTran.Primary.Stream = uint.Parse(valueList[1]);
                    oldSecsTran.Primary.Function = uint.Parse(valueList[2]);
                    oldSecsTran.Secondary.Stream = uint.Parse(valueList[1]);
                    oldSecsTran.Secondary.Function = uint.Parse(valueList[2]) + 1;
                }
                if (valueDic["desc"] == "" || valueDic["name"] == "")
                {
                    selectNode.Text = valueDic["name"] + " " + valueDic["desc"];

                }
                else
                {
                    selectNode.Text = valueDic["name"] + " - " + valueDic["desc"];
                }
            }
            catch (Exception)
            {
                oldSecsTran.Name = oldName;
                oldSecsTran.Description = oldDesc;
                throw new Exception("保存transaction失败，可能存在同名transaction");
            }
            if (valueDic["reply"] == "1")
            {
                oldSecsTran.ReplyExpected = true;
            }
            else
            {
                oldSecsTran.ReplyExpected = false;
            }
            if (valueDic["autoSyatem"] == "1")
            {
                oldSecsTran.AutoSystemBytes = true;
            }
            else
            {
                oldSecsTran.AutoSystemBytes = false;
            }
        }

        static private void SaveAndShowLevel2(MyEventArg arg)
        {
            SECSMessage secsMes = (SECSMessage)selectNode.Tag;
            Dictionary<string, string> valueDic = (Dictionary<string, string>)arg.Value;
            secsMes.Name = valueDic["name"];
            secsMes.Description = valueDic["desc"];
            secsMes.Stream = uint.Parse(valueDic["stream"]);
            secsMes.Function = uint.Parse(valueDic["function"]);
            if ((secsMes.Function & 1) == 1)
            {
                if (valueDic["desc"] == "" || valueDic["name"] == "")
                {
                    selectNode.Text = "P" + " " + valueDic["name"] + " " + valueDic["desc"];
                    
                }
                else
                {
                    selectNode.Text = "P" + " " + valueDic["name"] + " - " + valueDic["desc"];
                }
            }
            else
            {
                if (valueDic["desc"] == "" || valueDic["name"] == "")
                {
                    selectNode.Text = "S" + " " + valueDic["name"] + " " + valueDic["desc"];

                }
                else
                {
                    selectNode.Text = "S" + " " + valueDic["name"] + " - " + valueDic["desc"];
                }
            }
        }

        static private void SaveAndShowLevelN(MyEventArg arg)
        {
            SECSItem secsItem = (SECSItem)selectNode.Tag;
            Dictionary<string, object> valueDic = (Dictionary<string, object>)arg.Value;
            secsItem.Name = (string)valueDic["name"];
            secsItem.Description = (string)valueDic["desc"];
            if (secsItem.Format == SECS_FORMAT.LIST)
            {
                if ((string)valueDic["desc"] == "" || (string)valueDic["name"] == "")
                {
                    selectNode.Text = valueDic["name"] + " " + valueDic["desc"];

                }
                else
                {
                    selectNode.Text = (string)valueDic["name"] + " - " + (string)valueDic["desc"];
                }
                return;
            }
            secsItem.Format = (SECS_FORMAT)Enum.Parse(typeof(SECS_FORMAT), (string)valueDic["format"]);
            secsItem.NLB = (string)valueDic["nbl"] == "Auto" ? 0 : uint.Parse((string)valueDic["nbl"]);
            secsItem.Value = valueDic["value"];
            string format = secsItem.Format.ToString();
            if (secsItem.Format == SECS_FORMAT.ASCII)
            {
                format = "A";
            }
            else if (secsItem.Format == SECS_FORMAT.BINARY)
            {
                format = "B";
            }
            else if (secsItem.Format == SECS_FORMAT.JIS8)
            {
                format = "J";
            }
            if (valueDic["value"] == null)
            {
                if (secsItem.Description == "")
                {
                    selectNode.Text = format + " " + secsItem.Name + " = " + "<nothing>";
                }
                else
                {
                    selectNode.Text = format + " " + secsItem.Name + " = " + "<nothing>" + " - " + secsItem.Description;
                }
                return;
            }
            if (secsItem.Description == "")
            {
                if (secsItem.Value.GetType().BaseType == typeof(Array))
                {
                    selectNode.Text = format + " " + secsItem.Name + " = " + string.Join(" ", (int[])valueDic["value"]);
                }
                else
                {
                    if (format == "A" || format == "J")
                    {
                        selectNode.Text = format + " " + secsItem.Name + " = " + "'" + secsItem.Value.ToString() + "'";
                    }
                    else
                    {
                        selectNode.Text = format + " " + secsItem.Name + " = " + secsItem.Value.ToString();
                    }
                }
            }
            else
            {
                if (secsItem.Value.GetType().BaseType == typeof(Array))
                {
                    selectNode.Text = format + " " + secsItem.Name + " = " + string.Join(" ", (int[])valueDic["value"]) + " - " + secsItem.Description;
                }
                else
                {
                    if (format == "A" || format == "J")
                    {
                        selectNode.Text = format + " " + secsItem.Name + " = " + "'" + secsItem.Value.ToString() + "'" + " - " + secsItem.Description;
                    }
                    else
                    {
                        selectNode.Text = format + " " + secsItem.Name + " = " + secsItem.Value.ToString() + " - " + secsItem.Description;
                    }
                }
            }
        }

        static internal void SaveChildMessage(object sender, EventArgs e)
        {
            try
            {
                MyEventArg arg = e as MyEventArg;
                int level = selectNode.Level;
                switch (level)
                {
                    case 0:
                        secsLibrary.Description = arg.Value.ToString();
                        selectNode.Text = arg.Value.ToString();
                        break;
                    case 1:
                        SaveAndShowLevel1(arg);
                        break;
                    case 2:
                        SaveAndShowLevel2(arg);
                        break;
                    default:
                        SaveAndShowLevelN(arg);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddReceiveMessage(SECSTransaction transaction, bool flag)
        {
            SECSMessage message;
            string nodeText;
            if (flag == true)
            {
                message = transaction.Primary;
                nodeText = "P " + transaction.Name + " " + DateTime.Now.ToString("HH:mm:ss") + " - " + transaction.Description + " request";
            }
            else
            {
                message = transaction.Secondary;
                string funcName = "S" + message.Stream + "F" + message.Function;
                nodeText = "S " + funcName + " " + DateTime.Now.ToString("HH:mm:ss") + " " + " - " + transaction.Description + " acknowledge";
            }
            TreeNode node = NewSECSMessage(message, nodeText);
            messageTreeView.Nodes.Add(node);
            messageTreeView.ExpandAll();
        }

        private void AddErrorMessage(string message)
        {
            string strValue = DateTime.Now.ToString("HH:mm:ss.fff") + " " + message;
            listBox.Items.Add(strValue);
        }
        #endregion

        private void LibraryTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (propertyForm != null)
                {
                    propertyForm.Dispose();
                }
                return;
            }
            Point clickPoint = new Point(e.X, e.Y);
            selectNode = libraryTreeView.GetNodeAt(clickPoint);
            if (selectNode == null)
            {
                return;
            }
            if (selectNode.Level == 0)
            {
                propertiesToolStripMenuItem1.Enabled = true;
                sendMessageToolStripMenuItem.Enabled = false;
                //receiveMessageToolStripMenuItem.Enabled = false;
                //clearToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = false;
                insertTransactionToolStripMenuItem.Enabled = true;
                duplicateTransactionToolStripMenuItem.Enabled = false;
                insertChildItemToolStripMenuItem.Enabled = false;
                //insertSiblingItemToolStripMenuItem.Enabled = false;
                duplicateItemToolStripMenuItem.Enabled = false;
                //expandNodeToolStripMenuItem.Enabled = true;
                //collapseNodeToolStripMenuItem.Enabled = true;
                //useDefaultLibraryToolStripMenuItem.Enabled = true;
            }
            else if (selectNode.Level == 1)
            {
                propertiesToolStripMenuItem1.Enabled = true;
                sendMessageToolStripMenuItem.Enabled = true;
                //receiveMessageToolStripMenuItem.Enabled = true;
                //clearToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                insertTransactionToolStripMenuItem.Enabled = true;
                duplicateTransactionToolStripMenuItem.Enabled = true;
                insertChildItemToolStripMenuItem.Enabled = false;
                //insertSiblingItemToolStripMenuItem.Enabled = false;
                duplicateItemToolStripMenuItem.Enabled = false;
                //expandNodeToolStripMenuItem.Enabled = true;
                //collapseNodeToolStripMenuItem.Enabled = true;
                //useDefaultLibraryToolStripMenuItem.Enabled = true;
            }
            else if (selectNode.Level == 2)
            {
                propertiesToolStripMenuItem1.Enabled = true;
                sendMessageToolStripMenuItem.Enabled = false;
                //receiveMessageToolStripMenuItem.Enabled = false;
                //clearToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = false;
                insertTransactionToolStripMenuItem.Enabled = false;
                duplicateTransactionToolStripMenuItem.Enabled = false;
                insertChildItemToolStripMenuItem.Enabled = true;
                //insertSiblingItemToolStripMenuItem.Enabled = false;
                duplicateItemToolStripMenuItem.Enabled = false;
                //expandNodeToolStripMenuItem.Enabled = true;
                //collapseNodeToolStripMenuItem.Enabled = true;
                //useDefaultLibraryToolStripMenuItem.Enabled = true;
            }
            else
            {
                propertiesToolStripMenuItem1.Enabled = true;
                sendMessageToolStripMenuItem.Enabled = false;
                //receiveMessageToolStripMenuItem.Enabled = false;
                //clearToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                insertTransactionToolStripMenuItem.Enabled = false;
                duplicateTransactionToolStripMenuItem.Enabled = false;
                insertChildItemToolStripMenuItem.Enabled = true;
                //insertSiblingItemToolStripMenuItem.Enabled = true;
                duplicateItemToolStripMenuItem.Enabled = true;
                //expandNodeToolStripMenuItem.Enabled = true;
                //collapseNodeToolStripMenuItem.Enabled = true;
                //useDefaultLibraryToolStripMenuItem.Enabled = true;
            }
            selectNode.ContextMenuStrip = contextMenuStrip1;
        }

        private void InsertChildItem_Click(object sender, EventArgs e)
        {
            try
            {
                SECSItem newItem;
                char[] s = new char[] { '-', ' ' };
                string flg = selectNode.Text.Split(s)[0].Trim();
                if (flg == "P" || flg == "S")
                {
                    SECSMessage message = (SECSMessage)selectNode.Tag;
                    newItem = message.Root.AddNew("");
                }
                else
                {
                    SECSItem item = (SECSItem)selectNode.Tag;                 
                    selectNode.Text = "L";
                    newItem = item.AddNew("");
                }
                TreeNode node = new TreeNode();
                node.Text = newItem.Format + " " + newItem.Name + " = " + "<nothing>";
                node.Tag = newItem;
                selectNode.Nodes.Add(node);
            }
            catch (Exception)
            {
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {               
                if(selectNode.Level == 1)
                {
                    string tranName = GetTranName();
                    secsLibrary.Remove(tranName);
                }
                else
                {
                    SECSItem item = (SECSItem)selectNode.Tag;
                    item.Delete();
                }
                selectNode.Remove();
            }
            catch (Exception)
            {
            }
        }

        private void InsertTransaction_Click(object sender, EventArgs e)
        {
            try
            {             
                SECSTransaction newTran = new SECSTransaction(0, 2*functionNum+1);
                newTran.Description = "New SecsTransaction object";
                secsLibrary.Insert(newTran, selectNode.Index);

                TreeNode treeNode = GetNewNode(newTran.Name + " - " + newTran.Description, newTran);
                TreeNode pNode = GetNewNode("P - New SecsTransaction object request", newTran.Primary);
                TreeNode sNode = GetNewNode("S - New SecsTransaction object acknowledge", newTran.Secondary);

                treeNode.Nodes.Add(pNode);
                treeNode.Nodes.Add(sNode);
                if(selectNode.Level == 1)
                {
                    selectNode.Parent.Nodes.Insert(selectNode.Index, treeNode);
                }
                else
                {
                    selectNode.Nodes.Insert(0, treeNode);
                }
                functionNum += 1;
            }
            catch (Exception)
            {
            }
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            string tranName = GetTranName();
            SECSTransaction secsTran = secsLibrary.Find(tranName);
            secsTran.Send(winSecs);
            TreeNode primaryNode = (TreeNode)selectNode.FirstNode.Clone();
            primaryNode.Text = "P " + secsTran.Name + " " + DateTime.Now.ToString("HH:mm:ss") + " " + " - " + secsTran.Description + " request";
            messageTreeView.Nodes.Add(primaryNode);
            messageTreeView.ExpandAll();
        }

        private void UseDefaultLibrary_Click(object sender, EventArgs e)
        {
            GetNodeTree(GetDefaultFilePath());
        }

        private void OpenLibFile_Click(object sender, EventArgs e)
        {
            OpenFile("(*.lib)|*.lib");
        }

        private void OpenXmlFile_Click(object sender, EventArgs e)
        {
            OpenFile("(*.xml)|*.xml");
        }

        private void SaveLibFile_Click(object sender, EventArgs e)
        {
            SaveFile("(*.lib)|*.lib");
        }

        private void SaveXmlFile_Click(object sender, EventArgs e)
        {
            SaveFile("(*.xml)|*.xml");
        }

        private void PropertiesTool_Click(object sender, EventArgs e)
        {
            PropertyForm();
            DeleteCtlEvent();
            AddCtlEvent();
            selectCtl.Show();
            propertyForm.Show();
        }

        private void PropertiesToolStrip_Click(object sender, EventArgs e)
        {
            HSMSProperties hsmsGUI = new HSMSProperties();
            hsmsGUI.Show();
        }

        private void OpenTool_Click(object sender, EventArgs e)
        {
            winSecs.Connect();
            openToolStripMenuItem1.Enabled = false;
            closeToolStripMenuItem.Enabled = true;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            winSecs.Disconnect();
            openToolStripMenuItem1.Enabled = true;
            closeToolStripMenuItem.Enabled = false;
        }

        private void ClearToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
        }

        private void ClearToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            messageTreeView.Nodes.Clear();
        }

        private void LibraryTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(e.Action != TreeViewAction.ByMouse)
            {
                return;
            }
            if (e.Node.Checked)
            {
                sendMessageList.Add(e.Node);
                TreeNode node = (TreeNode)e.Node.Clone();
                sendListTreeView.Nodes.Add(node);
                return;
            }
            //sendMessageList.Remove(e.Node);
            //foreach (TreeNode node in sendListTreeView.Nodes)
            //{
            //    if (node == null)
            //    {
            //        return;
            //    }
            //    if (node.Text == e.Node.Text)
            //    {
            //        sendListTreeView.Nodes.Remove(node);
            //    }
            //}
            //sendListTreeView.Refresh();
        }

        private void SendTranListButton_Click(object sender, EventArgs e)
        {
            lock (sendMessageList)
            {
                winSecs.sendState.Set();
                while (sendMessageList.Count != 0)
                {
                    if (winSecs.sendState.WaitOne())
                    {
                        TreeNode node = sendMessageList[0];
                        string tranName = node.Text.Split('-')[0].Trim() + node.Text.Split('-')[1].Trim();
                        SECSTransaction secsTran = secsLibrary.Find(tranName);
                        secsTran.Send(winSecs);
                        TreeNode primaryNode = (TreeNode)node.FirstNode.Clone();
                        primaryNode.Text = "P " + secsTran.Name + " " + DateTime.Now.ToString("HH:mm:ss") + " " + " - " + secsTran.Description + " request";
                        messageTreeView.Nodes.Add(primaryNode);
                        messageTreeView.ExpandAll();
                        sendMessageList.RemoveAt(0);
                        libraryTreeView.Nodes[0].Nodes[node.Index].Checked = false;
                    }
                }
            }
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node1 in sendMessageList)
            {
                //foreach (TreeNode node2 in libraryTreeView.Nodes[0].Nodes)
                //{
                //    if (node1.Text == node2.Text)
                //    {
                //        node2.Checked = false;
                //    }
                //}
                libraryTreeView.Nodes[0].Nodes[node1.Index].Checked = false;
            }
            sendListTreeView.Nodes.Clear();
            sendMessageList.Clear();
        }

        private void DuplicateTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SECSTransaction secsTran = (SECSTransaction)selectNode.Tag;
            SECSTransaction newTran = secsTran.Duplicate();
            string uuid = Guid.NewGuid().ToString().Replace("-", "");
            //string uuid = Guid.NewGuid().ToString("X");
            newTran.Description = secsTran.Description + " " + uuid;
            secsLibrary.Insert(newTran, selectNode.Index);

            TreeNode tNode = GetNewNode(newTran.Name + " - " + newTran.Description, newTran);
            TreeNode pNode = NewSECSMessage(newTran.Primary, selectNode.FirstNode.Text);
            tNode.Nodes.Add(pNode);
            TreeNode sNode = NewSECSMessage(newTran.Secondary, selectNode.LastNode.Text);
            tNode.Nodes.Add(sNode);

            selectNode.Parent.Nodes.Insert(selectNode.Index, tNode);
        }

        private void DuplicateItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text;
            SECSItem secsItem = (SECSItem)selectNode.Tag;
            SECSItem newItem = secsItem.Duplicate();
            if(secsItem.Format == SECS_FORMAT.LIST)
            {
                text = GetLNodeText(newItem);
            }
            else
            {
                text = GetNNodeText(newItem);
            }
            TreeNode treeNode = GetNewNode(text, newItem);
            GetChildItem(newItem, treeNode.Nodes);
            selectNode.Parent.Nodes.Add(treeNode);
        }

        private void SendListTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            Point clickPoint = new Point(e.X, e.Y);
            selectNode = sendListTreeView.GetNodeAt(clickPoint);
            if (selectNode == null)
            {
                return;
            }
            selectNode.ContextMenuStrip = contextMenuStrip4;
        }
        private void DeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sendListTreeView.Nodes.Remove(selectNode);
            sendMessageList.RemoveAt(selectNode.Index);
        }

        #region 隐藏CheckNode
        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;
        private void HideCheckBox(TreeView tvw, TreeNode node)
        {

            TVITEM tvi = new TVITEM();

            tvi.hItem = node.Handle;

            tvi.mask = TVIF_STATE;

            tvi.stateMask = TVIS_STATEIMAGEMASK;

            tvi.state = 0;

            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);

        }

        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        private void LibraryTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //隐藏节点前的checkbox
            if (e.Node.Level != 1)
                //所有的节点都通过判断ImageIndex来决定
                //是否隐藏CheckBox，所以在添加节点时有意            
                //修改节点的ImageIndex = -1
                HideCheckBox(libraryTreeView, e.Node);
            e.DrawDefault = true;
        }

        private void FASTsim_Load(object sender, EventArgs e)
        {
            //在窗口或者Editor加载时设置treeview的绘制模式，改为手动绘制
            libraryTreeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }
        #endregion

        //private void SendTranListButton_Click(object sender, EventArgs e)
        //{
        //    lock (sendMessageList)
        //    {
        //        if (sendMessageList.Count != 0)
        //        {
        //            foreach (TreeNode node in sendMessageList)
        //            {
        //                string tranName = node.Text.Split('-')[0].Trim() + node.Text.Split('-')[1].Trim();
        //                SECSTransaction secsTran = secsLibrary.Find(tranName);
        //                secsTran.Send(winSecs);
        //                TreeNode primaryNode = (TreeNode)node.FirstNode.Clone();
        //                primaryNode.Text = "P " + secsTran.Name + " " + DateTime.Now.ToString("HH:mm:ss") + " " + " - " + secsTran.Description + " request";
        //                messageTreeView.Nodes.Add(primaryNode);
        //            }
        //        }
        //    }
        //    ClearSendList();
        //}
    }
}
