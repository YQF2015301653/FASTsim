using FASTsim.Library.SECS;
using System.Windows.Forms;

namespace FASTsim.GUI
{
    public partial class Level9 : Form
    {
        public Level9()
        {
            InitializeComponent();
        }


        //private InitControl()
        //{
        //    level9Format.i
        //}

        public void GetValue(object message)
        {
            SECSItem item = (SECSItem)message;
            level9Name.Text = item.Name;
            level9Descr.Text = item.Description;
            //level9CheckBox.Checked = item.f
        }

    }
}
