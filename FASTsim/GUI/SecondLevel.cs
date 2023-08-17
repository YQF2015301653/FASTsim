using FASTsim.Library.SECS;
using System;
using System.Collections.Generic;
using static FASTsim.Library.SECS.CommonData;

namespace FASTsim.GUI
{
    public partial class SecondLevel : CommonControl
    {
        public event EventHandler SendMsgEvent;
        public SecondLevel()
        {
            InitializeComponent();
            SendMsgEvent += FASTsim.SaveChildMessage;
        }
        internal override void MainFormNodeChaned(object sender, EventArgs e)
        {
            MyEventArg arg = e as MyEventArg;
            SECSMessage secsMes = (SECSMessage)arg.Value;
            level2Name.Text = secsMes.Name;
            level2Descr.Text = secsMes.Description;
            level2Fun.Text = secsMes.Function.ToString();
            level2Stream.Text = secsMes.Stream.ToString();
        }

        private void SendMessage()
        {
            if(SendMsgEvent != null)
            {
                Dictionary<string, string> valueDic = new Dictionary<string, string>();
                valueDic["name"] = level2Name.Text;
                valueDic["desc"] = level2Descr.Text;
                valueDic["stream"] = level2Stream.Text;
                valueDic["function"] = level2Fun.Text;
                SendMsgEvent(this, new MyEventArg() { Value =  valueDic});
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SendMessage();
            this.FindForm().Close();
        }
    }
}
