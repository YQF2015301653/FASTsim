using System;
using System.Collections;
namespace FASTsim.Library.SECS
{
    public class SECSItem
    {
        private string m_name;
        private string m_description;
        internal uint hackcount;
        private ArrayList m_itemlist;
        private SECS_FORMAT m_format;
        private uint m_NLB;
        private object m_value;
        private uint m_length;
        internal uint m_EncodingCode;
        internal int m_CodePage;
        internal SECSItem m_parent;

        public SECSItem(string name, string desc)
        {
            m_name = name;
            m_description = desc;
            m_itemlist = new ArrayList();
            m_format = SECS_FORMAT.UNDEFINED;
            hackcount = 0;
            m_EncodingCode = WsEncoding.DefaultEncodingCode();
        }

        ~SECSItem()
        {
        }

        #region 增删改查
        internal void AddFast(SECSItem child)
        {
            if (m_format != SECS_FORMAT.LIST)
            {
                throw new Exception("Can't add to a non-list item");
            }
            m_itemlist.Add(child);
            m_length++;
            child.m_parent = this;
            child.m_EncodingCode = m_EncodingCode;
        }

        public SECSItem AddNew(string name) => AddNew(name, "");

        public SECSItem AddNew(string name, string desc)
        {
            SECSItem item = new SECSItem(name, desc);
            if (m_format == SECS_FORMAT.UNDEFINED)
            {
                m_format = SECS_FORMAT.LIST;
            }
            if (m_format != SECS_FORMAT.LIST)
            {
                m_format = SECS_FORMAT.LIST;
                m_itemlist = new ArrayList();
            }
            m_itemlist.Add(item);
            m_length++;
            item.m_parent = this;
            item.m_EncodingCode = m_EncodingCode;
            return item;
        }

        internal SECSItem Clone()
        {
            SECSItem item = new SECSItem(Name, Description)
            {
                Description = Description,
                Name = Name,
                Format = m_format
            };
            if (m_format != SECS_FORMAT.LIST)
            {
                if (m_value != null)
                {
                    item.Value = m_value;
                }
            }
            else
            {
                for (int i = 0; i < ItemCount; i++)
                {
                    item.AddFast(((SECSItem)m_itemlist[i]).Clone());
                }
            }
            return item;
        }

        public SECSItem Duplicate()
        {
            if ((m_parent == null) || (m_parent.Format != SECS_FORMAT.LIST))
            {
                throw new Exception("Can not duplicate. Parent is not a list");
            }
            SECSItem child = new SECSItem(Name, Description)
            {
                Description = Description,
                Name = Name,
                Format = m_format
            };
            if (m_format != SECS_FORMAT.LIST)
            {
                if (m_value != null)
                {
                    child.Value = m_value;
                }
            }
            else
            {
                for (int i = 0; i < ItemCount; i++)
                {
                    child.AddFast(((SECSItem)m_itemlist[i]).Clone());
                }
            }
            m_parent.AddFast(child);
            return child;
        }

        public void Delete()
        {
            m_parent.Remove(this);
        }

        public void Remove(SECSItem item)
        {
            for (int i = 0; i < m_itemlist.Count; i++)
            {
                if (m_itemlist[i] == item)
                {
                    m_itemlist.Remove(item);
                    m_length--;
                    return;
                }
            }
            throw new Exception("Item not found");
        }

        public SECSItem Item(int nth) => (SECSItem)m_itemlist[nth];

        public SECSItem Item(string name)
        {
            for (int i = 0; i < m_itemlist.Count; i++)
            {
                if (((SECSItem)m_itemlist[i]).Name == name)
                {
                    return (SECSItem)m_itemlist[i];
                }
            }
            for (int j = 0; j < m_itemlist.Count; j++)
            {
                if (((SECSItem)m_itemlist[j]).Format == SECS_FORMAT.LIST)
                {
                    return ((SECSItem)m_itemlist[j]).Item(name);
                }
            }
            return null;
        }

        //public SECSItem Item(string name, int nth)
        //{
        //    int num = 0;
        //    for (int i = 0; i < m_itemlist.Count; i++)
        //    {
        //        if (((SECSItem)m_itemlist[i]).Name == name)
        //        {
        //            if (num == nth)
        //            {
        //                return (SECSItem)m_itemlist[i];
        //            }
        //            num++;
        //        }
        //    }
        //    return null;
        //}

        #endregion

        #region 解析Item
        internal static void ParseItem(WinSECS ws, ref SECSItem item, SECS_FORMAT format, ref byte[] src, int index, int length)
        {
            int num24;
            item.Format = format;
            SECS_FORMAT secs_format = item.Format;
            if (secs_format > SECS_FORMAT.I4)
            {
                if (secs_format == SECS_FORMAT.F8)
                {
                    int num28 = length / 8;
                    double[] numArray9 = new double[num28];
                    for (int j = 0; j < num28; j++)
                    {
                        byte[] buffer3 = new byte[src[index-1]];
                        //byte[] buffer3 = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                        buffer3[7] = src[index++];
                        buffer3[6] = src[index++];
                        buffer3[5] = src[index++];
                        buffer3[4] = src[index++];
                        buffer3[3] = src[index++];
                        buffer3[2] = src[index++];
                        buffer3[1] = src[index++];
                        numArray9[j] = BitConverter.ToDouble(buffer3, 0);
                    }
                    if (num28 == 1)
                    {
                        item.ParseResults(numArray9[0], (uint)length);
                        return;
                    }
                    item.ParseResults(numArray9, (uint)length);
                    return;
                }
                switch (secs_format)
                {
                    case SECS_FORMAT.F4:
                        {
                            int num26 = length / 4;
                            float[] numArray8 = new float[num26];
                            for (int j = 0; j < num26; j++)
                            {
                                byte[] buffer2 = new byte[src[index - 1]];
                                buffer2[3] = src[index++];
                                buffer2[2] = src[index++];
                                buffer2[1] = src[index++];
                                numArray8[j] = BitConverter.ToSingle(buffer2, 0);
                            }
                            if (num26 == 1)
                            {
                                item.ParseResults(numArray8[0], (uint)length);
                                return;
                            }
                            item.ParseResults(numArray8, (uint)length);
                            return;
                        }
                    case ((SECS_FORMAT)0x25):
                    case ((SECS_FORMAT)0x26):
                    case ((SECS_FORMAT)0x27):
                    case ((SECS_FORMAT)0x2b):
                        break;

                    case SECS_FORMAT.U8:
                        {
                            int num4 = length / 8;
                            ulong[] numArray = new ulong[num4];
                            for (int j = 0; j < num4; j++)
                            {
                                numArray[j] = (ulong)((((((((src[index++] << 0x38) | (src[index++] << 0x30)) | (src[index++] << 40)) | (src[index++] << 0x20)) | (src[index++] << 0x18)) | (src[index++] << 0x10)) | (src[index++] << 8)) | src[index++]);
                            }
                            if (num4 == 1)
                            {
                                item.ParseResults(numArray[0], (uint)length);
                                return;
                            }
                            item.ParseResults(numArray, (uint)length);
                            return;
                        }
                    case SECS_FORMAT.U1:
                        goto TR_002E;

                    case SECS_FORMAT.U2:
                        {
                            int num16 = length / 2;
                            ushort[] numArray5 = new ushort[num16];
                            for (int j = 0; j < num16; j++)
                            {
                                numArray5[j] = (ushort)(((ushort)(src[index++] << 8)) | src[index++]);
                            }
                            if (num16 == 1)
                            {
                                item.ParseResults(numArray5[0], (uint)length);
                                return;
                            }
                            item.ParseResults(numArray5, (uint)length);
                            return;
                        }
                    case SECS_FORMAT.U4:
                        {
                            int num10 = length / 4;
                            uint[] numArray3 = new uint[num10];
                            for (int j = 0; j < num10; j++)
                            {
                                numArray3[j] = (uint)((((src[index++] << 0x18) | (src[index++] << 0x10)) | (src[index++] << 8)) | src[index++]);
                            }
                            if (num10 == 1)
                            {
                                item.ParseResults(numArray3[0], (uint)length);
                                return;
                            }
                            item.ParseResults(numArray3, (uint)length);
                            return;
                        }
                    default:
                        if (secs_format == SECS_FORMAT.UNDEFINED)
                        {
                            throw new Exception("Item '" + item.Name + "' has underfined format");
                        }
                        return;
                }
                return;
            }
            else
            {
                switch (secs_format)
                {
                    case SECS_FORMAT.BINARY:
                        break;

                    case SECS_FORMAT.BOOLEAN:
                        {
                            int num30 = length;
                            bool[] flagArray = new bool[num30];
                            for (int j = 0; j < num30; j++)
                            {
                                flagArray[j] = src[index++] != 0;
                            }
                            if (num30 == 1)
                            {
                                item.ParseResults(flagArray[0], (uint)length);
                                return;
                            }
                            item.ParseResults(flagArray, (uint)length);
                            return;
                        }
                    default:
                        switch (secs_format)
                        {
                            case SECS_FORMAT.ASCII:
                                {
                                    string sOut = "";
                                    WsEncoding.BytesToString(ws.CodePage, src, index, length, out sOut);
                                    item.ParseResults(sOut, (uint)length);
                                    return;
                                }
                            case SECS_FORMAT.JIS8:
                                {
                                    string str2 = "";
                                    for (int j = 0; j < length; j++)
                                    {
                                        char ch = (char)(((ushort)(src[index++] << 8)) | src[index++]);
                                        str2 = str2 + ch;
                                    }
                                    item.ParseResults(str2, (uint)length);
                                    return;
                                }
                            case SECS_FORMAT.CHAR2:
                                {
                                    string str3;
                                    ushort encodingCode = 0;
                                    encodingCode = (ushort)(src[index++] << 8);
                                    encodingCode = src[index++];
                                    if (!WsEncoding.BytesToString((int)WsEncoding.GetCodePage(encodingCode), src, index, length - 2, out str3))
                                    {
                                        break;
                                    }
                                    item.ParseResults(str3, (uint)length);
                                    ws.EncodingCode = encodingCode;
                                    return;
                                }
                            case ((SECS_FORMAT)0x13):
                            case ((SECS_FORMAT)20):
                            case ((SECS_FORMAT)0x15):
                            case ((SECS_FORMAT)0x16):
                            case ((SECS_FORMAT)0x17):
                            case ((SECS_FORMAT)0x1b):
                                break;

                            case SECS_FORMAT.I8:
                                {
                                    int num7 = length / 8;
                                    long[] numArray2 = new long[num7];
                                    for (int j = 0; j < num7; j++)
                                    {
                                        numArray2[j] = (((((((src[index++] << 0x38) | (src[index++] << 0x30)) | (src[index++] << 40)) | (src[index++] << 0x20)) | (src[index++] << 0x18)) | (src[index++] << 0x10)) | (src[index++] << 8)) | src[index++];
                                    }
                                    if (num7 == 1)
                                    {
                                        item.ParseResults(numArray2[0], (uint)length);
                                        return;
                                    }
                                    item.ParseResults(numArray2, (uint)length);
                                    return;
                                }
                            case SECS_FORMAT.I1:
                                {
                                    int num22 = length;
                                    sbyte[] numArray7 = new sbyte[num22];
                                    for (int j = 0; j < num22; j++)
                                    {
                                        numArray7[j] = (sbyte)src[index++];
                                    }
                                    if (num22 == 1)
                                    {
                                        item.ParseResults(numArray7[0], (uint)length);
                                        return;
                                    }
                                    item.ParseResults(numArray7, (uint)length);
                                    return;
                                }
                            case SECS_FORMAT.I2:
                                {
                                    int num19 = length / 2;
                                    short[] numArray6 = new short[num19];
                                    for (int j = 0; j < num19; j++)
                                    {
                                        ushort num21 = (ushort)(((ushort)(src[index++] << 8)) | src[index++]);
                                        numArray6[j] = (short)num21;
                                    }
                                    if (num19 == 1)
                                    {
                                        item.ParseResults(numArray6[0], (uint)length);
                                        return;
                                    }
                                    item.ParseResults(numArray6, (uint)length);
                                    return;
                                }
                            case SECS_FORMAT.I4:
                                {
                                    int num13 = length / 4;
                                    int[] numArray4 = new int[num13];
                                    for (int j = 0; j < num13; j++)
                                    {
                                        numArray4[j] = (((src[index++] << 0x18) | (src[index++] << 0x10)) | (src[index++] << 8)) | src[index++];
                                    }
                                    if (num13 == 1)
                                    {
                                        item.ParseResults(numArray4[0], (uint)length);
                                        return;
                                    }
                                    item.ParseResults(numArray4, (uint)length);
                                    return;
                                }
                            default:
                                return;
                        }
                        return;
                }
            }
        TR_002E:
            num24 = length;
            byte[] valu = new byte[num24];
            for (int i = 0; i < num24; i++)
            {
                valu[i] = src[index++];
            }
            if (num24 == 1)
            {
                item.ParseResults(valu[0], (uint)length);
            }
            else
            {
                item.ParseResults(valu, (uint)length);
            }
        }

        internal void ParseResults(object valu, uint length)
        {
            m_value = valu;
            m_length = length;
        }
        #endregion

        #region 属性值
        public string Name
        {
            get =>
                m_name;
            set =>
                m_name = value;
        }

        public string Description
        {
            get =>
                m_description;
            set =>
                m_description = value;
        }

        public SECS_FORMAT Format
        {
            get =>
                m_format;
            set =>
                m_format = value;
        }

        public int ItemCount => m_itemlist.Count;

        public uint Length
        {
            get
            {
                if (Format == SECS_FORMAT.CHAR2)
                {
                    int num;
                    int codePage = -1;
                    codePage = (m_EncodingCode == 0) ? WsEncoding.DefaultCodePage() : ((int)WsEncoding.GetCodePage((int)m_EncodingCode));
                    m_length = !WsEncoding.GetByteCount(codePage, Value.ToString(), out num) ? 0 : ((uint)(num + 2));
                }
                else if (Format == SECS_FORMAT.ASCII)
                {
                    int num3;
                    if(Value == null)
                    {
                        Value = "";
                    }
                    m_length = !WsEncoding.GetByteCount(m_CodePage, Value.ToString(), out num3) ? 0 : ((uint)num3);
                }
                uint num4 = (uint)((m_length >= 0x100) ? ((m_length >= 0x10000) ? 3 : 2) : 1);
                return ((m_format != SECS_FORMAT.LIST) ? ((1 + num4) + m_length) : (1 + num4));
            }
        }

        public uint NLB
        {
            get =>
                m_NLB;
            set
            {
                if (value > 3)
                {
                    throw new Exception("NLB must be 0, 1, 2, or 3");
                }
                m_NLB = value;
            }
        }

        public SECSItem Parent
        {
            get =>
                m_parent;
            set
            {
                SECSItem child = value;
                if (IsChild(child))
                {
                    throw new Exception("Circular parentage is not possible");
                }
                m_parent.Remove(this);
                try
                {
                    child.AddFast(this);
                }
                catch (Exception exception)
                {
                    m_parent.AddFast(this);
                    throw exception;
                }
            }
        }

        public object Value
        {
            get =>
                m_value;
            set
            {
                try
                {
                    if(value == null)
                    {
                        return;
                    }
                    uint length = 1;
                    if (value is Array)
                    {
                        length = (uint)((Array)value).Length;
                    }
                    /*如果m_format为UNDEFINED,通过值的类型确定其数据格式*/
                    if (m_format == SECS_FORMAT.UNDEFINED)
                    {
                        Type objA = value.GetType();
                        if (objA.IsArray)
                        {
                            objA = objA.GetElementType();
                        }
                        if (ReferenceEquals(objA, Type.GetType("System.Boolean")))
                        {
                            m_format = SECS_FORMAT.BOOLEAN;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.Byte")))
                        {
                            m_format = SECS_FORMAT.U1;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.SByte")))
                        {
                            m_format = SECS_FORMAT.I1;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.Double")))
                        {
                            m_format = SECS_FORMAT.F8;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.Single")))
                        {
                            m_format = SECS_FORMAT.F4;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.Int32")))
                        {
                            m_format = SECS_FORMAT.I4;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.UInt32")))
                        {
                            m_format = SECS_FORMAT.U4;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.Int64")))
                        {
                            m_format = SECS_FORMAT.I8;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.UInt64")))
                        {
                            m_format = SECS_FORMAT.U8;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.Int16")))
                        {
                            m_format = SECS_FORMAT.I2;
                        }
                        else if (ReferenceEquals(objA, Type.GetType("System.UInt16")))
                        {
                            m_format = SECS_FORMAT.U2;
                        }
                        else
                        {
                            if (!ReferenceEquals(objA, Type.GetType("System.String")))
                            {
                                throw new Exception("Unknown type");
                            }
                            m_format = SECS_FORMAT.ASCII;
                        }
                    }
                    /*确定数据的长度*/
                    SECS_FORMAT format = m_format;
                    if (format > SECS_FORMAT.I4)
                    {
                        if (format == SECS_FORMAT.F8)
                        {
                            m_length = 8 * length;
                        }
                        else
                        {
                            switch (format)
                            {
                                case SECS_FORMAT.F4:
                                    m_length = 4 * length;
                                    break;

                                case SECS_FORMAT.U8:
                                    m_length = 8 * length;
                                    break;

                                case SECS_FORMAT.U1:
                                    m_length = length;
                                    break;

                                case SECS_FORMAT.U2:
                                    m_length = 2 * length;
                                    break;

                                case SECS_FORMAT.U4:
                                    m_length = 4 * length;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (format)
                        {
                            case SECS_FORMAT.BINARY:
                                m_length = length;
                                break;

                            case SECS_FORMAT.BOOLEAN:
                                m_length = length;
                                break;

                            default:
                                switch (format)
                                {
                                    case SECS_FORMAT.ASCII:
                                        int num2;
                                        m_length = !WsEncoding.GetByteCount(WsEncoding.DefaultCodePage(), value.ToString(), out num2) ? 0 : ((uint)num2);
                                        break;

                                    case SECS_FORMAT.JIS8:
                                        m_length = (uint)(value.ToString().Length * 2);
                                        break;

                                    case SECS_FORMAT.CHAR2:
                                        int num3;
                                        m_length = !WsEncoding.GetByteCount((int)WsEncoding.GetCodePage((int)m_EncodingCode), value.ToString(), out num3) ? 0 : ((uint)(num3 + 2));
                                        break;

                                    case SECS_FORMAT.I8:
                                        m_length = 8 * length;
                                        break;

                                    case SECS_FORMAT.I1:
                                        m_length = length;
                                        break;

                                    case SECS_FORMAT.I2:
                                        m_length = 2 * length;
                                        break;

                                    case SECS_FORMAT.I4:
                                        m_length = 4 * length;
                                        break;

                                    default:
                                        break;
                                }
                                break;
                        }
                    }
                    /*获取数据的值*/
                    format = m_format;
                    if (format > SECS_FORMAT.I4)
                    {
                        if (format == SECS_FORMAT.F8)
                        {
                            if (!(value is Array))
                            {
                                m_value = Convert.ToDouble(value);
                            }
                            else
                            {
                                double[] numArray = new double[length];
                                for (int i = 0; i < length; i++)
                                {
                                    numArray[i] = Convert.ToDouble(((Array)value).GetValue(i));
                                }
                                m_value = numArray;
                            }
                        }
                        else
                        {
                            switch (format)
                            {
                                case SECS_FORMAT.F4:
                                    {
                                        if (!(value is Array))
                                        {
                                            m_value = Convert.ToSingle(value);
                                            return;
                                        }
                                        float[] numArray5 = new float[length];
                                        for (int i = 0; i < length; i++)
                                        {
                                            numArray5[i] = Convert.ToSingle(((Array)value).GetValue(i));
                                        }
                                        m_value = numArray5;
                                        return;
                                    }
                                case (SECS_FORMAT)0x25:
                                case (SECS_FORMAT)0x26:
                                case (SECS_FORMAT)0x27:
                                case (SECS_FORMAT)0x2b:
                                    break;

                                case SECS_FORMAT.U8:
                                    {
                                        if (!(value is Array))
                                        {
                                            m_value = Convert.ToUInt64(value);
                                            return;
                                        }
                                        ulong[] numArray2 = new ulong[length];
                                        for (int i = 0; i < length; i++)
                                        {
                                            numArray2[i] = Convert.ToUInt64(((Array)value).GetValue(i));
                                        }
                                        m_value = numArray2;
                                        return;
                                    }
                                case SECS_FORMAT.U1:
                                    {
                                        if (!(value is Array))
                                        {
                                            m_value = Convert.ToByte(value);
                                            return;
                                        }
                                        byte[] buffer2 = new byte[length];
                                        for (int i = 0; i < length; i++)
                                        {
                                            buffer2[i] = Convert.ToByte(((Array)value).GetValue(i));
                                        }
                                        m_value = buffer2;
                                        return;
                                    }
                                case SECS_FORMAT.U2:
                                    {
                                        if (!(value is Array))
                                        {
                                            m_value = Convert.ToUInt16(value);
                                            break;
                                        }
                                        ushort[] numArray9 = new ushort[length];
                                        for (int i = 0; i < length; i++)
                                        {
                                            numArray9[i] = Convert.ToUInt16(((Array)value).GetValue(i));
                                        }
                                        m_value = numArray9;
                                        return;
                                    }
                                case SECS_FORMAT.U4:
                                    {
                                        if (!(value is Array))
                                        {
                                            m_value = Convert.ToUInt32(value);
                                            return;
                                        }
                                        uint[] numArray7 = new uint[length];
                                        for (int i = 0; i < length; i++)
                                        {
                                            numArray7[i] = Convert.ToUInt32(((Array)value).GetValue(i));
                                        }
                                        m_value = numArray7;
                                        return;
                                    }
                                default:
                                    return;
                            }
                        }
                    }
                    else
                    {
                        switch (format)
                        {
                            case SECS_FORMAT.BINARY:
                                {
                                    if (!(value is Array))
                                    {
                                        if (value is string)
                                        {
                                            m_value = value;
                                            return;
                                        }
                                        m_value = Convert.ToByte(value);
                                        return;
                                    }
                                    byte[] buffer = new byte[length];
                                    for (int i = 0; i < length; i++)
                                    {
                                        buffer[i] = Convert.ToByte(((Array)value).GetValue(i));
                                    }
                                    m_value = buffer;
                                    return;
                                }
                            case SECS_FORMAT.BOOLEAN:
                                {
                                    if (!(value is Array))
                                    {
                                        m_value = Convert.ToBoolean(value);
                                        return;
                                    }
                                    bool[] flagArray = new bool[length];
                                    for (int i = 0; i < length; i++)
                                    {
                                        flagArray[i] = Convert.ToBoolean(((Array)value).GetValue(i));
                                    }
                                    m_value = flagArray;
                                    return;
                                }
                            default:
                                switch (format)
                                {
                                    case SECS_FORMAT.ASCII:
                                        if (value is Array)
                                        {
                                            throw new Exception("Can not convert an array to ASCII");
                                        }
                                        m_value = value;
                                        return;

                                    case SECS_FORMAT.JIS8:
                                        if (value is Array)
                                        {
                                            throw new Exception("Can not convert an array to JIS8");
                                        }
                                        m_value = value;
                                        return;

                                    case SECS_FORMAT.CHAR2:
                                        if (value is Array)
                                        {
                                            throw new Exception("Can not convert an array to ASCII");
                                        }
                                        m_value = value;
                                        return;

                                    case (SECS_FORMAT)0x13:
                                    case (SECS_FORMAT)20:
                                    case (SECS_FORMAT)0x15:
                                    case (SECS_FORMAT)0x16:
                                    case (SECS_FORMAT)0x17:
                                    case (SECS_FORMAT)0x1b:
                                        break;

                                    case SECS_FORMAT.I8:
                                        {
                                            if (!(value is Array))
                                            {
                                                m_value = Convert.ToInt64(value);
                                                return;
                                            }
                                            long[] numArray3 = new long[length];
                                            for (int i = 0; i < length; i++)
                                            {
                                                numArray3[i] = Convert.ToInt64(((Array)value).GetValue(i));
                                            }
                                            m_value = numArray3;
                                            return;
                                        }
                                    case SECS_FORMAT.I1:
                                        {
                                            if (!(value is Array))
                                            {
                                                m_value = Convert.ToSByte(value);
                                                return;
                                            }
                                            sbyte[] numArray4 = new sbyte[length];
                                            for (int i = 0; i < length; i++)
                                            {
                                                numArray4[i] = Convert.ToSByte(((Array)value).GetValue(i));
                                            }
                                            m_value = numArray4;
                                            return;
                                        }
                                    case SECS_FORMAT.I2:
                                        {
                                            if (!(value is Array))
                                            {
                                                m_value = Convert.ToInt16(value);
                                                return;
                                            }
                                            short[] numArray8 = new short[length];
                                            for (int i = 0; i < length; i++)
                                            {
                                                numArray8[i] = Convert.ToInt16(((Array)value).GetValue(i));
                                            }
                                            m_value = numArray8;
                                            return;
                                        }
                                    case SECS_FORMAT.I4:
                                        {
                                            if (!(value is Array))
                                            {
                                                m_value = Convert.ToInt32(value);
                                                return;
                                            }
                                            int[] numArray6 = new int[length];
                                            for (int i = 0; i < length; i++)
                                            {
                                                numArray6[i] = Convert.ToInt32(((Array)value).GetValue(i));
                                            }
                                            m_value = numArray6;
                                            return;
                                        }
                                    default:
                                        return;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {                  
                    throw ex;
                }
            }
        }
        #endregion

        public bool IsChild(SECSItem Child)
        {
            if (m_format == SECS_FORMAT.LIST)
            {
                for (int i = 0; i < m_itemlist.Count; i++)
                {
                    if (ReferenceEquals((SECSItem)m_itemlist[i], Child))
                    {
                        return true;
                    }
                    if (IsChild((SECSItem)m_itemlist[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public byte[] Raw()
        //{
        //    int index = 0;
        //    byte num2 = (byte)((m_length >= 0x100) ? ((m_length >= 0x10000) ? 3 : 2) : 1);
        //    byte[] dst = (m_format != SECS_FORMAT.LIST) ? new byte[(1 + num2) + m_length] : new byte[1 + num2];
        //    Raw(ref dst, ref index);
        //    return dst;
        //}

        internal void Raw(ref byte[] dst, ref int index)
        {
            byte[] buffer4;
            int num23;
            IntPtr ptr;
            byte num = (byte)((m_length >= 0x100) ? ((m_length >= 0x10000) ? 3 : 2) : 1);
            dst[index] = num;
            index = (num23 = index) + 1;
            (buffer4 = dst)[(int)(ptr = (IntPtr)num23)] = (byte)(buffer4[(int)ptr] | ((byte)(((byte)m_format) << 2)));
            if (num == 1)
            {
                index = (num23 = index) + 1;
                dst[num23] = (byte)m_length;
            }
            else if (num == 2)
            {
                index = (num23 = index) + 1;
                dst[num23] = (byte)((m_length & 0xff00) >> 8);
                index = (num23 = index) + 1;
                dst[num23] = (byte)(m_length & 0xff);
            }
            else
            {
                index = (num23 = index) + 1;
                dst[num23] = (byte)((m_length & 0xff0000) >> 0x10);
                index = (num23 = index) + 1;
                dst[num23] = (byte)((m_length & 0xff00) >> 8);
                index = (num23 = index) + 1;
                dst[num23] = (byte)(m_length & 0xff);
            }
            if (m_length == 0)
            {
                return;
            }
            SECS_FORMAT format = m_format;
            if (format > SECS_FORMAT.I4)
            {
                if (format == SECS_FORMAT.F8)
                {
                    double[] numArray9 = !(m_value is Array) ? new double[] { ((double)m_value) } : ((double[])m_value);
                    for (int i = 0; i < numArray9.Length; i++)
                    {
                        byte[] bytes = BitConverter.GetBytes(numArray9[i]);
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[7];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[6];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[5];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[4];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[3];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[2];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[1];
                        index = (num23 = index) + 1;
                        dst[num23] = bytes[0];
                    }
                    return;
                }
                switch (format)
                {
                    case SECS_FORMAT.F4:
                        {
                            float[] numArray8 = !(m_value is Array) ? new float[] { ((float)m_value) } : ((float[])m_value);
                            for (int i = 0; i < numArray8.Length; i++)
                            {
                                byte[] bytes = BitConverter.GetBytes(numArray8[i]);
                                index = (num23 = index) + 1;
                                dst[num23] = bytes[3];
                                index = (num23 = index) + 1;
                                dst[num23] = bytes[2];
                                index = (num23 = index) + 1;
                                dst[num23] = bytes[1];
                                index = (num23 = index) + 1;
                                dst[num23] = bytes[0];
                            }
                            return;
                        }
                    case ((SECS_FORMAT)0x25):
                    case ((SECS_FORMAT)0x26):
                    case ((SECS_FORMAT)0x27):
                    case ((SECS_FORMAT)0x2b):
                        break;

                    case SECS_FORMAT.U8:
                        foreach (ulong num7 in !(m_value is Array) ? new ulong[] { (ulong)m_value } : ((ulong[])m_value))
                        {
                            index = (num23 = index) + 1;
                            /*可能有问题*/
                            dst[num23] = (byte)((num7 & 72057594037927936L) >> 0x38);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num7 & 0xff000000000000L) >> 0x30);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num7 & 0xff0000000000L) >> 40);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num7 & 0xff00000000L) >> 0x20);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num7 & 0xff000000UL) >> 0x18);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num7 & 0xff0000L) >> 0x10);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num7 & 0xff00L) >> 8);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)(num7 & ((ulong)0xffL));
                        }
                        return;

                    case SECS_FORMAT.U1:
                        goto TR_002B;

                    case SECS_FORMAT.U2:
                        foreach (ushort num15 in !(m_value is Array) ? new ushort[] { ((ushort)m_value) } : ((ushort[])m_value))
                        {
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num15 & 0xff00) >> 8);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)(num15 & 0xff);
                        }
                        return;

                    case SECS_FORMAT.U4:
                        foreach (uint num11 in !(m_value is Array) ? new uint[] { ((uint)m_value) } : ((uint[])m_value))
                        {
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num11 & -16777216) >> 0x18);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num11 & 0xff0000) >> 0x10);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)((num11 & 0xff00) >> 8);
                            index = (num23 = index) + 1;
                            dst[num23] = (byte)(num11 & 0xff);
                        }
                        return;

                    default:
                        return;
                }
                return;
            }
            else
            {
                switch (format)
                {
                    case SECS_FORMAT.BINARY:
                        goto TR_002B;

                    case SECS_FORMAT.BOOLEAN:
                        {
                            bool[] flagArray = !(m_value is Array) ? new bool[] { ((bool)m_value) } : ((bool[])m_value);
                            for (int i = 0; i < flagArray.Length; i++)
                            {
                                index = (num23 = index) + 1;
                                dst[num23] = flagArray[i] ? ((byte)1) : ((byte)0);
                            }
                            break;
                        }
                    default:
                        switch (format)
                        {
                            case SECS_FORMAT.ASCII:
                                {
                                    string sIn = (string)m_value;
                                    int iLength = 0;
                                    if (!WsEncoding.StringToBytes(m_CodePage, sIn, index, ref dst, ref iLength))
                                    {
                                        break;
                                    }
                                    index += iLength;
                                    return;
                                }
                            case SECS_FORMAT.JIS8:
                                foreach (char ch in (string)m_value)
                                {
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((ch & 0xff00) >> 8);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)(ch & '\x00ff');
                                }
                                return;

                            case SECS_FORMAT.CHAR2:
                                {
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((m_EncodingCode & 0xff00) >> 8);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)(m_EncodingCode & 0xff);
                                    int iLength = 0;
                                    if (!WsEncoding.StringToBytes((int)WsEncoding.GetCodePage((int)m_EncodingCode), (string)m_value, index, ref dst, ref iLength))
                                    {
                                        break;
                                    }
                                    index += iLength;
                                    return;
                                }
                            case ((SECS_FORMAT)0x13):
                            case ((SECS_FORMAT)20):
                            case ((SECS_FORMAT)0x15):
                            case ((SECS_FORMAT)0x16):
                            case ((SECS_FORMAT)0x17):
                            case ((SECS_FORMAT)0x1b):
                                break;

                            case SECS_FORMAT.I8:
                                foreach (ulong num9 in !(m_value is Array) ? new long[] { ((long)m_value) } : ((long[])m_value))
                                {
                                    index = (num23 = index) + 1;
                                    /*可能有问题*/
                                    dst[num23] = (byte)((num9 & 72057594037927936L) >> 0x38);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num9 & 0xff000000000000L) >> 0x30);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num9 & 0xff0000000000L) >> 40);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num9 & 0xff00000000L) >> 0x20);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num9 & 0xff000000UL) >> 0x18);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num9 & 0xff0000L) >> 0x10);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num9 & 0xff00L) >> 8);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)(num9 & ((ulong)0xffL));
                                }
                                return;

                            case SECS_FORMAT.I1:
                                {
                                    if (!(m_value is Array))
                                    {
                                        index = (num23 = index) + 1;
                                        dst[num23] = (byte)((sbyte)m_value);
                                        return;
                                    }
                                    sbyte[] numArray7 = (sbyte[])m_value;
                                    for (int i = 0; i < numArray7.Length; i++)
                                    {
                                        index = (num23 = index) + 1;
                                        dst[num23] = (byte)numArray7[i];
                                    }
                                    return;
                                }
                            case SECS_FORMAT.I2:
                                foreach (ushort num17 in !(m_value is Array) ? new short[] { ((short)m_value) } : ((short[])m_value))
                                {
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num17 & 0xff00) >> 8);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)(num17 & 0xff);
                                }
                                return;

                            case SECS_FORMAT.I4:
                                foreach (uint num13 in !(m_value is Array) ? new int[] { ((int)m_value) } : ((int[])m_value))
                                {
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num13 & -16777216) >> 0x18);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num13 & 0xff0000) >> 0x10);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)((num13 & 0xff00) >> 8);
                                    index = (num23 = index) + 1;
                                    dst[num23] = (byte)(num13 & 0xff);
                                }
                                return;

                            default:
                                return;
                        }
                        break;
                }
                return;
            }
        TR_002B:
            if (!(m_value is Array))
            {
                index = (num23 = index) + 1;
                dst[num23] = (byte)m_value;
            }
            else
            {
                byte[] buffer = (byte[])m_value;
                for (int i = 0; i < buffer.Length; i++)
                {
                    index = (num23 = index) + 1;
                    dst[num23] = buffer[i];
                }
            }
        }
    }
}
