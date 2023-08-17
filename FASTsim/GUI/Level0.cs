using FASTsim.Library.SECS;
using System.Windows.Forms;

namespace FASTsim.GUI
{
    public partial class Level0 : Form
    {
        public Level0()
        {
            InitializeComponent();
        }

        public void GetValue(object message)
        {
            SECSLibrary t = (SECSLibrary)message;
            level0Descr.Text = t.Description;
        }

        private void level0Descr_TextChanged(object sender, System.EventArgs e)      
        {
            string descrText = level0Descr.Text;
        }
    }
}
