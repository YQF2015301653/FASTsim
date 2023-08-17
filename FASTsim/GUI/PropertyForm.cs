using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace FASTsim.GUI
{
    public partial class PropertyForm : Form
    {
        public Control[] mCtrl;

        public PropertyForm()
        {
            InitializeComponent();
            InitSubGUI();
        }

        private void InitSubGUI()
        {
            mCtrl = new Control[4];

            mCtrl[0] = new ZeroLevel();
            mCtrl[0].Dock = DockStyle.Fill;
            AddControl(mCtrl[0]);

            mCtrl[1] = new FirstLevel();
            mCtrl[1].Dock = DockStyle.Fill;
            AddControl(mCtrl[1]);

            mCtrl[2] = new SecondLevel();
            mCtrl[2].Dock = DockStyle.Fill;
            AddControl(mCtrl[2]);

            mCtrl[3] = new OtherLevel();
            mCtrl[3].Dock = DockStyle.Fill;
            AddControl(mCtrl[3]);

            for (int i = 0; i < mCtrl.Length; i++)
                mCtrl[i].Visible = false;
        }


        public void AddControl(Control ctl)
        {
            panel.Controls.Add(ctl);
        }
    }
}
