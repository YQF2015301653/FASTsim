using FASTsim.Library.SECS;
using System;
using System.IO;

namespace FASTsim.Library
{
    public class HSMSConfig : ParameterBase
    {
        private ushort mDeviceId;
        private int mT3Timeout;
        private int mT5Timeout;
        private int mT6Timeout;
        private int mT7Timeout;
        private int mT8Timeout;
        private int mLinktestInterval;
        private bool mLinktestEnabled;

        private string mIPAddress;
        private int mPortNo;

        private HSMS_CONNECTION_MODE mMode;

        private bool mLogEnable;

        public bool LogEnable
        {
            get { return mLogEnable; }
            set { mLogEnable = value; }
        }

        private bool mControlMessageLogEnable;

        public bool ControlMessageLogEnable
        {
            get { return mControlMessageLogEnable; }
            set { mControlMessageLogEnable = value; }
        }

        private bool mRawLogEnable;

        public bool RawLogEnable
        {
            get { return mRawLogEnable; }
            set { mRawLogEnable = value; }
        }



        public HSMSConfig()
        {
            mKey = Path.Combine(mFilePath, "\\HSMSConfig.xml");
            mMode = HSMS_CONNECTION_MODE.ACTIVE;
            mIPAddress = "127.0.0.1";
            mPortNo = 5000;

            mT3Timeout = 45;
            mT5Timeout = 10;
            mT6Timeout = 5;
            mT7Timeout = 10;
            mT8Timeout = 5;
            mLinktestInterval = 10;
            mLinktestEnabled = false;
        }

        public ushort DeviceId
        {
            get
            {
                return mDeviceId;
            }
            set
            {
                if (value < 0 || value > 32767)
                {
                    throw new Exception("Value is out of range 0 - 32767");
                }
                mDeviceId = value;
            }
        }

        public int T3Timeout
        {
            get
            {
                return mT3Timeout;
            }
            set
            {
                if (value < 1 || value > 120)
                {
                    throw new Exception("Value is out of range 1 - 120 seconds");
                }
                mT3Timeout = value;
            }
        }

        public int T5Timeout
        {
            get
            {
                return mT5Timeout;
            }
            set
            {
                if (value < 1 || value > 240)
                {
                    throw new Exception("Value is out of range 1 - 240 seconds");
                }
                mT5Timeout = value;
            }
        }

        public int T6Timeout
        {
            get
            {
                return mT6Timeout;
            }
            set
            {
                if (value < 1 || value > 240)
                {
                    throw new Exception("Value is out of range 1 - 240 seconds");
                }
                mT6Timeout = value;
            }
        }

        public int T7Timeout
        {
            get
            {
                return mT7Timeout;
            }
            set
            {
                if (value < 1 || value > 240)
                {
                    throw new Exception("Value is out of range 1 - 240 seconds");
                }
                mT7Timeout = value;
            }
        }

        public int T8Timeout
        {
            get
            {
                return mT8Timeout;
            }
            set
            {
                if (value < 1 || value > 120)
                {
                    throw new Exception("Value is out of range 1 - 120 seconds");
                }
                mT8Timeout = value;
            }
        }

        public int LinktestInterval
        {
            get
            {
                return mLinktestInterval;
            }
            set
            {
                if (value < 0 || value > 900)
                {
                    throw new Exception("Value is out of range 0 - 900 seconds");
                }
                mLinktestInterval = value;
            }
        }

        public bool LinktestEnabled
        {
            get
            {
                return mLinktestEnabled;
            }

            set
            {
                mLinktestEnabled = value;
            }
        }

        public HSMS_CONNECTION_MODE Mode
        {
            get
            {
                return mMode;
            }
            set
            {
                mMode = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return mIPAddress;
            }
            set
            {
                mIPAddress = value;
            }

        }

        /// <summary>
        /// Remote or Local Port 
        /// </summary>
        public int PortNo
        {
            get
            {
                return mPortNo;
            }
            set
            {
                mPortNo = value;
            }
        }
    }
}
