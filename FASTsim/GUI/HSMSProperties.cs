using FASTsim.Library;
using FASTsim.Library.SECS;
using System;
using System.Windows.Forms;

namespace FASTsim.GUI
{
    public partial class HSMSProperties : Form
    {
        private HSMSConfig mHSMSConfig;
        public HSMSProperties()
        {
            InitializeComponent();
            mHSMSConfig = new HSMSConfig().Read() as HSMSConfig;
        }

        private void HSMSProperties_Load(object sender, System.EventArgs e)
        {
            try
            {
                numT3.Value = mHSMSConfig.T3Timeout;
                numT5.Value = mHSMSConfig.T5Timeout;
                numT6.Value = mHSMSConfig.T6Timeout;
                numT7.Value = mHSMSConfig.T7Timeout;
                numT8.Value = mHSMSConfig.T8Timeout;
                numCircuit.Value = mHSMSConfig.LinktestInterval;
                numDeviceID.Value = mHSMSConfig.DeviceId;
                numTCPPort.Value = mHSMSConfig.PortNo;

                string[] strTempSplit = mHSMSConfig.IPAddress.Split('.');
                numIP1.Value = Convert.ToInt32(strTempSplit[0]);
                numIP2.Value = Convert.ToInt32(strTempSplit[1]);
                numIP3.Value = Convert.ToInt32(strTempSplit[2]);
                numIP4.Value = Convert.ToInt32(strTempSplit[3]);
                rbtActive.Checked = mHSMSConfig.Mode == HSMS_CONNECTION_MODE.ACTIVE;
                rbtPassive.Checked = !rbtActive.Checked;
            }
            catch (Exception ex)
            {
            }
        }

        private void ApplyButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                applyButton.Enabled = false;
                if (rbtActive.Checked)
                    mHSMSConfig.Mode = HSMS_CONNECTION_MODE.ACTIVE;
                else
                    mHSMSConfig.Mode = HSMS_CONNECTION_MODE.PASSIVE;

                mHSMSConfig.IPAddress = numIP1.Value.ToString() + "." + numIP2.Value.ToString()
                    + "." + numIP3.Value.ToString() + "." + numIP4.Value.ToString();

                mHSMSConfig.PortNo = Convert.ToInt32(numTCPPort.Value);
                mHSMSConfig.T3Timeout = Convert.ToInt32(numT3.Value);
                mHSMSConfig.T5Timeout = Convert.ToInt32(numT5.Value);
                mHSMSConfig.T6Timeout = Convert.ToInt32(numT6.Value);
                mHSMSConfig.T7Timeout = Convert.ToInt32(numT7.Value);
                mHSMSConfig.T8Timeout = Convert.ToInt32(numT8.Value);
                mHSMSConfig.LinktestInterval = Convert.ToInt32(numCircuit.Value);
                mHSMSConfig.DeviceId = Convert.ToUInt16(numDeviceID.Value);
                mHSMSConfig.Save();

                applyButton.Enabled = true;
                this.Close();
            }
            catch (Exception ex)
            {
                applyButton.Enabled = true;
            }
        }
    }
}
