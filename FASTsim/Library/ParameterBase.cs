using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

namespace FASTsim.Library
{
    public class ParameterBase
    {
        protected string mFilePath = null;
        protected string mKey = null;
        protected string mDefaultKey = null;

        private string mCreator = null;
        private DateTime mTime;


        public ParameterBase()
        {
            mFilePath = AppDomain.CurrentDomain.BaseDirectory + "config";
            mTime = DateTime.Now;
        }

        [XmlIgnore]
        [Browsable(false)]
        public string Key
        {
            get { return mKey; }
            set { mKey = value; }
        }


        [XmlElementAttribute(IsNullable = false)]
        [Category("Create Information")]
        public string Creator
        {
            get { return mCreator; }
            set { mCreator = value; }
        }


        [Category("Create Information")]
        public DateTime CreateTime
        {
            get { return mTime; }
            set { mTime = value; }
        }

        public virtual void InitParam()
        {
        }

        public virtual void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());

                using (StreamWriter write = new StreamWriter(mKey))
                {
                    serializer.Serialize(write, this);
                    write.Close();
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }

        public virtual object Read()
        {
            try
            {
                if (mKey == null || !File.Exists(mKey))
                {
                    this.InitParam();
                    return this;
                }

                XmlSerializer serializer = new XmlSerializer(this.GetType());
                object temp = null;

                if (File.Exists(mKey))
                {
                    using (StreamReader read = new StreamReader(mKey))
                    {
                        temp = serializer.Deserialize(read);
                        read.Close();
                    }

                    return temp;
                }
                else
                {
                    this.InitParam();
                    return this;
                }
            }
            catch
            {
                this.InitParam();
                return this;
            }
        }
    }
}
