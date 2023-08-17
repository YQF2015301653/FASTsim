using System;
using System.Windows.Forms;

namespace FASTsim.GUI
{
    public partial class CommonControl : UserControl
    {
        public CommonControl()
        {
            InitializeComponent();
        }
        internal virtual void MainFormNodeChaned(object sender, EventArgs e)
        {
            throw new Exception();
        }
    }
}
