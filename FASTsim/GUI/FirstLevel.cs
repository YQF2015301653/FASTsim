using FASTsim.Library.SECS;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static FASTsim.Library.SECS.CommonData;

namespace FASTsim.GUI
{
    public partial class FirstLevel : CommonControl
    {
        public event EventHandler SendMsgEvent;

        public FirstLevel()
        {
            InitializeComponent();
            SendMsgEvent += FASTsim.SaveChildMessage;
        }
        internal override void MainFormNodeChaned(object sender, EventArgs e)
        {
            MyEventArg arg = e as MyEventArg;
            SECSTransaction secsTran = (SECSTransaction)arg.Value;
            level1Name.Text = secsTran.Name;
            level1Descr.Text = secsTran.Description;
            level1ReplyExpected.Checked = secsTran.ReplyExpected;
            level1AutoSystemBytes.Checked = secsTran.AutoSystemBytes;
        }
        private void SendMessage()
        {
            try
            {
                if (SendMsgEvent != null)
                {
                    Dictionary<string, string> valueDic = new Dictionary<string, string>();
                    valueDic["name"] = level1Name.Text;
                    valueDic["desc"] = level1Descr.Text;
                    if (level1ReplyExpected.Checked == true)
                    {
                        valueDic["reply"] = "1";
                    }
                    else
                    {
                        valueDic["reply"] = "0";
                    }
                    if (level1AutoSystemBytes.Checked == true)
                    {
                        valueDic["autoSyatem"] = "1";
                    }
                    else
                    {
                        valueDic["autoSyatem"] = "0";
                    }
                    SendMsgEvent(this, new MyEventArg() { Value = valueDic });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "，建议命名格式为: " + "S" + "n" + "F" + "n");
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SendMessage();
            this.FindForm().Close();
        }
    }
}
