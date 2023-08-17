using System;
using static FASTsim.Library.SECS.CommonData;

namespace FASTsim.GUI
{
    public partial class ZeroLevel : CommonControl
    {
        public event EventHandler SendMsgEvent;

        public ZeroLevel()
        {
            InitializeComponent();
        }
        internal override void MainFormNodeChaned(object sender, EventArgs e)
        {
            MyEventArg arg = e as MyEventArg;
            level0Descr.Text = (string)arg.Value;
        }

        private void Level0Descr_TextChanged(object sender, EventArgs e)
        {
            SendMsgEvent += FASTsim.SaveChildMessage;
            SendMsgEvent(this, new MyEventArg() { Value = level0Descr.Text });
        }
    }
}
