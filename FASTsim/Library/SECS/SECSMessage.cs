using System.Runtime.InteropServices;

namespace FASTsim.Library.SECS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SECSMESSAGE
    {
        public uint Size;
        public uint DeviceId;
        public uint Stream;
        public uint Function;
        public uint System;
        //public uint Flags;
        //public uint Tag;
        //public uint T3;
        public uint IsReplyExpected;
        public uint transactionId;
    }

    public class SECSMessage
    {
        // Fields
        private string m_name;
        private string m_description;
        private SECSItem m_rootitem;
        private uint m_stream;
        private uint m_function;

        // Methods
        public SECSMessage(string name, string desc)
        {
            m_name = name;
            m_description = desc;
            m_rootitem = new SECSItem("Root", "Root");
            m_rootitem.Format = SECS_FORMAT.LIST;
        }

        internal void Clonefrom(SECSMessage src)
        {
            m_name = src.Name;
            m_description = src.Description;
            m_stream = src.Stream;
            m_function = src.Function;
            m_rootitem = src.m_rootitem.Clone();
        }

        ~SECSMessage()
        {
        }

        #region 计算当前Item及其子节点中共有多少字节的数据
        private uint HelpCalcLength(SECSItem s)
        {
            uint length = s.Length;
            if (s.Format == SECS_FORMAT.LIST)
            {
                for (int i = 0; i < s.ItemCount; i++)
                {
                    length += HelpCalcLength(s.Item(i));
                }
            }
            return length;
        }

        private uint HelpCalcRootLength(SECSItem s)
        {
            uint num = 0;
            if (s.Format != SECS_FORMAT.LIST)
            {
                num += s.Length;
            }
            else
            {
                for (int i = 0; i < s.ItemCount; i++)
                {
                    num += HelpCalcLength(s.Item(i));
                }
            }
            return num;
        }
        #endregion

        #region 属性值
        public uint Stream
        {
            get =>
                m_stream;
            set =>
                m_stream = value;
        }

        public uint Function
        {
            get =>
                m_function;
            set =>
                m_function = value;
        }

        public string Name
        {
            get => m_name;
            set => m_name = value;
        }
        public string Description
        {
            get => m_description;
            set => m_description = value;
        }

        public SECSItem Root => m_rootitem;

        public uint Length => HelpCalcRootLength(m_rootitem);
        #endregion

        public void SetRoot(SECSItem item)
        {
            m_rootitem = null;
            m_rootitem = item;
        }
    }
}

