using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace FASTsim.Library.SECS
{
    public class SECSLibrary
    {
        public Coll m_list;
        private string m_name;
        private string m_description;
        private Hashtable hashtable1;

        public SECSLibrary(string name, string desc)
        {
            m_name = name;
            m_description = desc;
            m_list = new Coll();
        }

        #region XML数据格式解析
        public bool Load(string filename)
        {
            if(filename == "")
            {
                return false;
            }
            using(XmlTextReader r = new XmlTextReader(filename))
            {
                while (r.Read())
                {
                    XmlNodeType nodeType = r.NodeType;
                    switch (nodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (r.Name == "Library")
                                {
                                    ParseLib(r);
                                    return true;
                                }
                                continue;
                            }
                        case XmlNodeType.Attribute:
                            {
                                continue;
                            }
                        case XmlNodeType.Text:
                            {
                                Console.WriteLine("Text:" + r.Value);
                                continue;
                            }
                    }
                }
            }
            return true;
        }


        private string ParseGetElementTextValue(XmlTextReader r)
        {
            string str = "";
            if (r.IsEmptyElement)
            {
                return str;
            }
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Text)
                {
                    str += r.Value;
                    continue;
                }
                if (r.NodeType == XmlNodeType.EndElement)
                {
                    return str;
                }
            }
            return "";
        }

        private SECS_FORMAT ParseGetFormatValue(XmlTextReader r)
        {
            if (hashtable1 == null)
            {
                hashtable1 = new Hashtable(0x2a, 0.5f);
                hashtable1.Add("LIST", 0);
                hashtable1.Add("List", 1);
                hashtable1.Add("ASCII", 2);
                hashtable1.Add("BINARY", 3);
                hashtable1.Add("Binary", 4);
                hashtable1.Add("BOOLEAN", 5);
                hashtable1.Add("Boolean", 6);
                hashtable1.Add("I1", 7);
                hashtable1.Add("I2", 8);
                hashtable1.Add("I4", (int)9);
                hashtable1.Add("I8", (int)10);
                hashtable1.Add("U1", (int)11);
                hashtable1.Add("U2", (int)12);
                hashtable1.Add("U4", (int)13);
                hashtable1.Add("U8", (int)14);
                hashtable1.Add("F4", (int)15);
                hashtable1.Add("F8", (int)0x10);
                hashtable1.Add("JIS-8", (int)0x11);
                hashtable1.Add("Jis-8", (int)0x12);
                hashtable1.Add("JIS8", (int)0x13);
                hashtable1.Add("Jis8", (int)20);
            }
            SECS_FORMAT uNDEFINED = SECS_FORMAT.UNDEFINED;
            object obj2 = ParseGetElementTextValue(r);
            if (obj2 != null)
            {
                obj2 = hashtable1[obj2];
                if (obj2 != null)
                {
                    switch ((int)obj2)
                    {
                        case 0:
                        case 1:
                            uNDEFINED = SECS_FORMAT.LIST;
                            break;

                        case 2:
                            uNDEFINED = SECS_FORMAT.ASCII;
                            break;

                        case 3:
                        case 4:
                            uNDEFINED = SECS_FORMAT.BINARY;
                            break;

                        case 5:
                        case 6:
                            uNDEFINED = SECS_FORMAT.BOOLEAN;
                            break;

                        case 7:
                            uNDEFINED = SECS_FORMAT.I1;
                            break;

                        case 8:
                            uNDEFINED = SECS_FORMAT.I2;
                            break;

                        case 9:
                            uNDEFINED = SECS_FORMAT.I4;
                            break;

                        case 10:
                            uNDEFINED = SECS_FORMAT.I8;
                            break;

                        case 11:
                            uNDEFINED = SECS_FORMAT.U1;
                            break;

                        case 12:
                            uNDEFINED = SECS_FORMAT.U2;
                            break;

                        case 13:
                            uNDEFINED = SECS_FORMAT.U4;
                            break;

                        case 14:
                            uNDEFINED = SECS_FORMAT.U8;
                            break;

                        case 15:
                            uNDEFINED = SECS_FORMAT.F4;
                            break;

                        case 0x10:
                            uNDEFINED = SECS_FORMAT.F8;
                            break;

                        case 0x11:
                        case 0x12:
                        case 0x13:
                        case 20:
                            uNDEFINED = SECS_FORMAT.JIS8;
                            break;

                        default:
                            break;
                    }
                }
            }
            return uNDEFINED;
        }

        private bool ParseGetItemValue(XmlTextReader r, SECSItem item)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                {
                    if (r.Name == "Name")
                    {
                        item.Name = ParseGetElementTextValue(r);
                        continue;
                    }
                    if (r.Name == "Description")
                    {
                        item.Description = ParseGetElementTextValue(r);
                        continue;
                    }
                    if (r.Name == "Format")
                    {
                        item.Format = ParseGetFormatValue(r);
                        continue;
                    }
                    if (r.Name == "NLB")
                    {
                        uint num = Convert.ToUInt32(ParseGetElementTextValue(r));
                        if (num < 1)
                        {
                            num = 1;
                        }
                        if (num > 3)
                        {
                            num = 3;
                        }
                        item.NLB = num;
                        continue;
                    }
                    if (r.Name == "Value")
                    {
                        string s = ParseGetElementTextValue(r);
                        if (s.Length <= 0)
                        {
                            continue;
                        }
                        Cvt(item, s);
                        continue;
                    }
                    if (r.Name == "Item")
                    {
                        SECSItem child = new SECSItem("", "");
                        item.AddFast(child);
                        ParseGetItemValue(r, child);
                        continue;
                    }
                }
                if (r.NodeType == XmlNodeType.EndElement)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ParseGetMessageValue(XmlTextReader r, SECSTransaction t)
        {
            SECSItem item = (r.Name == "Primary") ? t.Primary.Root : t.Secondary.Root;
            SECSMessage secsMes = (r.Name == "Primary") ? t.Primary : t.Secondary;
            while (r.Read())
            {
                if((r.NodeType == XmlNodeType.Element) && (r.Name == "Name"))
                {
                    secsMes.Name = ParseGetElementTextValue(r);
                    continue;
                }
                if ((r.NodeType == XmlNodeType.Element) && (r.Name == "Description"))
                {
                    secsMes.Description = ParseGetElementTextValue(r);
                    continue;
                }
                if ((r.NodeType == XmlNodeType.Element) && (r.Name == "Item"))
                {
                    SECSItem child = new SECSItem("", "");
                    item.AddFast(child);
                    ParseGetItemValue(r, child);
                    continue;
                }
                if (r.NodeType == XmlNodeType.EndElement)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ParseLib(XmlTextReader r)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                {
                    if (r.Name == "Name")
                    {
                        m_name = ParseGetElementTextValue(r);
                        continue;
                    }
                    if (r.Name == "Description")
                    {
                        m_description = ParseGetElementTextValue(r);
                        continue;
                    }
                    if (r.Name == "Transaction")
                    {
                        SECSTransaction transaction = ParseTrans(r);
                        string keyName = transaction.Name + transaction.Description;
                        if (transaction == null)
                        {
                            break;
                        }
                        m_list.Add(keyName, transaction);
                        continue;
                    }
                }
                if (r.NodeType == XmlNodeType.EndElement)
                {
                    return true;
                }
            }
            return false;
        }

        private SECSTransaction ParseTrans(XmlTextReader r)
        {
            string str = "";
            uint stream = 0;
            uint function = 0;
            bool flag = false;
            SECSTransaction t = null;
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                {
                    if (r.Name == "Name")
                    {
                        ParseGetElementTextValue(r);
                        continue;
                    }
                    if (r.Name == "Description")
                    {
                        str = ParseGetElementTextValue(r);
                        continue;
                    }
                    if (r.Name == "Stream")
                    {
                        stream = Convert.ToUInt32(ParseGetElementTextValue(r));
                        continue;
                    }
                    if (r.Name == "Function")
                    {
                        function = Convert.ToUInt32(ParseGetElementTextValue(r));
                        continue;
                    }
                    if (r.Name == "ReplyExpected")
                    {
                        flag = Convert.ToBoolean(ParseGetElementTextValue(r));
                        continue;
                    }
                    if ((r.Name == "Primary") || (r.Name == "Secondary"))
                    {
                        if (t == null)
                        {
                            t = new SECSTransaction(stream, function)
                            {
                                Description = str,
                                ReplyExpected = flag
                            };
                        }
                        ParseGetMessageValue(r, t);
                        continue;
                    }
                }
                if (r.NodeType == XmlNodeType.EndElement)
                {
                    return t;
                }
            }
            return t;
        }

        /// <summary>
        /// 对item的数据解析保存到item.Value中
        /// </summary>
        /// <param name="item"></param>
        /// <param name="s">待处理的数据</param>
        /// <returns></returns>
        private bool Cvt(SECSItem item, string s)
        {
            uint num = (item.Value != null) ? (!(item.Value is Array) ? 2 : ((uint)(((Array)item.Value).Length + 1))) : 1;
            SECS_FORMAT format = item.Format;
            if (format > SECS_FORMAT.JIS8)
            {
                switch (format)
                {
                    case SECS_FORMAT.I8:
                        if (num == 1)
                        {
                            item.Value = Convert.ToInt64(s);
                        }
                        else if (num == 2)
                        {
                            long[] numArray6 = new long[] { (long)item.Value, Convert.ToInt64(s) };
                            item.Value = numArray6;
                        }
                        else
                        {
                            long[] numArray7 = (long[])item.Value;
                            long[] numArray8 = new long[num];
                            int index = 0;
                            while (true)
                            {
                                if (index >= (num - 1))
                                {
                                    numArray8[index] = Convert.ToInt64(s);
                                    item.Value = numArray8;
                                    break;
                                }
                                numArray8[index] = numArray7[index];
                                index++;
                            }
                        }
                        goto TR_0000;

                    case SECS_FORMAT.I1:
                        if (num == 1)
                        {
                            item.Value = Convert.ToSByte(s);
                        }
                        else if (num == 2)
                        {
                            sbyte[] numArray9 = new sbyte[] { (sbyte)item.Value, Convert.ToSByte(s) };
                            item.Value = numArray9;
                        }
                        else
                        {
                            sbyte[] numArray10 = (sbyte[])item.Value;
                            sbyte[] numArray11 = new sbyte[num];
                            int index = 0;
                            while (true)
                            {
                                if (index >= (num - 1))
                                {
                                    numArray11[index] = Convert.ToSByte(s);
                                    item.Value = numArray11;
                                    break;
                                }
                                numArray11[index] = numArray10[index];
                                index++;
                            }
                        }
                        goto TR_0000;

                    case SECS_FORMAT.I2:
                        if (num == 1)
                        {
                            item.Value = Convert.ToInt16(s);
                        }
                        else if (num == 2)
                        {
                            short[] numArray21 = new short[] { (short)item.Value, Convert.ToInt16(s) };
                            item.Value = numArray21;
                        }
                        else
                        {
                            short[] numArray22 = (short[])item.Value;
                            short[] numArray23 = new short[num];
                            int index = 0;
                            while (true)
                            {
                                if (index >= (num - 1))
                                {
                                    numArray23[index] = Convert.ToInt16(s);
                                    item.Value = numArray23;
                                    break;
                                }
                                numArray23[index] = numArray22[index];
                                index++;
                            }
                        }
                        goto TR_0000;

                    case ((SECS_FORMAT)0x1b):
                    case ((SECS_FORMAT)0x1d):
                    case ((SECS_FORMAT)30):
                    case ((SECS_FORMAT)0x1f):
                        goto TR_0000;

                    case SECS_FORMAT.I4:
                        if (num == 1)
                        {
                            item.Value = Convert.ToInt32(s);
                        }
                        else if (num == 2)
                        {
                            int[] numArray15 = new int[] { (int)item.Value, Convert.ToInt32(s) };
                            item.Value = numArray15;
                        }
                        else
                        {
                            int[] numArray16 = (int[])item.Value;
                            int[] numArray17 = new int[num];
                            int index = 0;
                            while (true)
                            {
                                if (index >= (num - 1))
                                {
                                    numArray17[index] = Convert.ToInt32(s);
                                    item.Value = numArray17;
                                    break;
                                }
                                numArray17[index] = numArray16[index];
                                index++;
                            }
                        }
                        goto TR_0000;

                    case SECS_FORMAT.F8:
                        if (num == 1)
                        {
                            item.Value = Convert.ToDouble(s);
                        }
                        else if (num == 2)
                        {
                            double[] numArray = new double[] { (double)item.Value, Convert.ToDouble(s) };
                            item.Value = numArray;
                        }
                        else
                        {
                            double[] numArray2 = (double[])item.Value;
                            double[] numArray3 = new double[num];
                            int index = 0;
                            while (true)
                            {
                                if (index >= (num - 1))
                                {
                                    numArray3[index] = Convert.ToDouble(s);
                                    item.Value = numArray3;
                                    break;
                                }
                                numArray3[index] = numArray2[index];
                                index++;
                            }
                        }
                        goto TR_0000;

                    default:
                        switch (format)
                        {
                            case SECS_FORMAT.F4:
                                if (num == 1)
                                {
                                    item.Value = Convert.ToSingle(s);
                                }
                                else if (num == 2)
                                {
                                    float[] numArray12 = new float[] { (float)item.Value, Convert.ToSingle(s) };
                                    item.Value = numArray12;
                                }
                                else
                                {
                                    float[] numArray13 = (float[])item.Value;
                                    float[] numArray14 = new float[num];
                                    int index = 0;
                                    while (true)
                                    {
                                        if (index >= (num - 1))
                                        {
                                            numArray14[index] = Convert.ToSingle(s);
                                            item.Value = numArray14;
                                            break;
                                        }
                                        numArray14[index] = numArray13[index];
                                        index++;
                                    }
                                }
                                goto TR_0000;

                            case SECS_FORMAT.U8:
                                if (num <= 1)
                                {
                                    item.Value = Convert.ToUInt64(s);
                                }
                                else
                                {
                                    ulong[] numArray4 = (ulong[])item.Value;
                                    ulong[] numArray5 = new ulong[num];
                                    int index = 0;
                                    while (true)
                                    {
                                        if (index >= (num - 1))
                                        {
                                            numArray5[index] = Convert.ToUInt64(s);
                                            item.Value = numArray5;
                                            break;
                                        }
                                        numArray5[index] = numArray4[index];
                                        index++;
                                    }
                                }
                                goto TR_0000;

                            case SECS_FORMAT.U1:
                                break;

                            case SECS_FORMAT.U2:
                                if (num == 1)
                                {
                                    item.Value = Convert.ToUInt16(s);
                                }
                                else if (num == 2)
                                {
                                    ushort[] numArray24 = new ushort[] { (ushort)item.Value, Convert.ToUInt16(s) };
                                    item.Value = numArray24;
                                }
                                else
                                {
                                    ushort[] numArray25 = (ushort[])item.Value;
                                    ushort[] numArray26 = new ushort[num];
                                    int index = 0;
                                    while (true)
                                    {
                                        if (index >= (num - 1))
                                        {
                                            numArray26[index] = Convert.ToUInt16(s);
                                            item.Value = numArray26;
                                            break;
                                        }
                                        numArray26[index] = numArray25[index];
                                        index++;
                                    }
                                }
                                goto TR_0000;

                            case SECS_FORMAT.U4:
                                if (num == 1)
                                {
                                    item.Value = Convert.ToUInt32(s);
                                }
                                else if (num == 2)
                                {
                                    uint[] numArray18 = new uint[] { (uint)item.Value, Convert.ToUInt32(s) };
                                    item.Value = numArray18;
                                }
                                else
                                {
                                    uint[] numArray19 = (uint[])item.Value;
                                    uint[] numArray20 = new uint[num];
                                    int index = 0;
                                    while (true)
                                    {
                                        if (index >= (num - 1))
                                        {
                                            numArray20[index] = Convert.ToUInt32(s);
                                            item.Value = numArray20;
                                            break;
                                        }
                                        numArray20[index] = numArray19[index];
                                        index++;
                                    }
                                }
                                goto TR_0000;

                            default:
                                goto TR_0000;
                        }
                        break;
                }
            }
            else
            {
                switch (format)
                {
                    case SECS_FORMAT.BINARY:
                        break;

                    case SECS_FORMAT.BOOLEAN:
                        if (num == 1)
                        {
                            item.Value = Convert.ToBoolean(s);
                        }
                        else if (num == 2)
                        {
                            bool[] flagArray = new bool[] { (bool)item.Value, Convert.ToBoolean(s) };
                            item.Value = flagArray;
                        }
                        else
                        {
                            bool[] flagArray2 = (bool[])item.Value;
                            bool[] flagArray3 = new bool[num];
                            int index = 0;
                            while (true)
                            {
                                if (index >= (num - 1))
                                {
                                    flagArray3[index] = Convert.ToBoolean(s);
                                    item.Value = flagArray3;
                                    break;
                                }
                                flagArray3[index] = flagArray2[index];
                                index++;
                            }
                        }
                        goto TR_0000;

                    default:
                        switch (format)
                        {
                            case SECS_FORMAT.ASCII:
                            case SECS_FORMAT.JIS8:
                                item.Value = s;
                                break;

                            default:
                                break;
                        }
                        goto TR_0000;
                }
            }
            if (num == 1)
            {
                item.Value = Convert.ToByte(s);
            }
            else if (num == 2)
            {
                byte[] buffer = new byte[] { (byte)item.Value, Convert.ToByte(s) };
                item.Value = buffer;
            }
            else
            {
                byte[] buffer2 = (byte[])item.Value;
                byte[] buffer3 = new byte[num];
                int index = 0;
                while (true)
                {
                    if (index >= (num - 1))
                    {
                        buffer3[index] = Convert.ToByte(s);
                        item.Value = buffer3;
                        break;
                    }
                    buffer3[index] = buffer2[index];
                    index++;
                }
            }
        TR_0000:
            return true;
        }
        #endregion

        #region 增删改查
        public void Add(SECSTransaction t)
        {
            m_list.Add(t.Name, t);
        }
        public void Insert(SECSTransaction t)
        {
            m_list.Insert(t.Name + t.Description, t);
        }

        public void Insert(SECSTransaction t, int index)
        {
            m_list.Insert(t.Name + t.Description, index, t);
        }

        public SECSTransaction Find(object sKey) => ((SECSTransaction)m_list[sKey]).Duplicate();
        public SECSTransaction Find(string sKey) => ((SECSTransaction)m_list[sKey]);

        public void Remove(string name)
        {
            m_list.Remove(name);
        }
        #endregion

        #region 保存Library树节点
        public void Save(string filename)
        {
            XmlTextWriter w = new XmlTextWriter(filename, Encoding.UTF8) { Formatting = Formatting.Indented };
            w.WriteStartElement("Library");
            w.WriteElementString("Name", Name);
            w.WriteElementString("Description", Description);
            for (int i = 0; i < m_list.Count; i++)
            {
                SECSTransaction t = (SECSTransaction)m_list[i];
                w.WriteStartElement("Transaction");
                SaveTrans(w, t);
                w.WriteEndElement();
            }
            w.WriteEndElement();
            w.Close();
        }

        private void SaveItem(XmlTextWriter w, SECSItem item)
        {
            w.WriteStartElement("Item");
            w.WriteElementString("Name", item.Name);
            w.WriteElementString("Description", item.Description);
            w.WriteElementString("Format", item.Format.ToString());
            if (item.Format == SECS_FORMAT.LIST)
            {
                for (int i = 0; i < item.ItemCount; i++)
                {
                    SaveItem(w, item.Item(i));
                }
            }
            else
            {
                object obj2 = item.Value;
                if (obj2 == null)
                {
                    w.WriteElementString("Value", "");
                }
                else if (!(obj2 is Array))
                {
                    w.WriteElementString("Value", obj2.ToString());
                }
                else
                {
                    Array array = (Array)obj2;
                    for (int i = 0; i < array.Length; i++)
                    {
                        w.WriteElementString("Value", array.GetValue(i).ToString());
                    }
                }
            }
            w.WriteEndElement();
        }

        private void SaveTrans(XmlTextWriter w, SECSTransaction t)
        {
            w.WriteElementString("Name", t.Name);
            w.WriteElementString("Description", t.Description);
            w.WriteElementString("Stream", t.Primary.Stream.ToString());
            w.WriteElementString("Function", t.Primary.Function.ToString());
            w.WriteElementString("ReplyExpected", t.ReplyExpected.ToString());
            w.WriteStartElement("Primary");
            w.WriteElementString("Name", t.Primary.Name);
            w.WriteElementString("Description", t.Primary.Description);
            if (t.Primary.Root.ItemCount > 0)
            {
                for (int i = 0; i < t.Primary.Root.ItemCount; i++)
                {
                    SaveItem(w, t.Primary.Root.Item(i));
                }
            }
            w.WriteFullEndElement();
            w.WriteStartElement("Secondary");
            w.WriteElementString("Name", t.Secondary.Name);
            w.WriteElementString("Description", t.Secondary.Description);
            if (t.Secondary.Root.ItemCount > 0)
            {
                for (int i = 0; i < t.Secondary.Root.ItemCount; i++)
                {
                    SaveItem(w, t.Secondary.Root.Item(i));
                }
            }
            w.WriteFullEndElement();
        }
        #endregion

        #region 数据结构转换
        public string Trans2XML(SECSTransaction t)
        {
            StringWriter w = new StringWriter();
            XmlTextWriter writer2 = new XmlTextWriter(w)
            {
                Formatting = Formatting.Indented
            };
            writer2.WriteStartElement("Transaction");
            SaveTrans(writer2, t);
            writer2.WriteEndElement();
            writer2.Close();
            return w.ToString();
        }

        public SECSTransaction XML2Trans(string s)
        {
            XmlTextReader r = new XmlTextReader(new StringReader(s));
            SECSTransaction transaction = null;
            while (true)
            {
                if (r.Read())
                {
                    if ((r.NodeType == XmlNodeType.Element) && (r.Name == "Transaction"))
                    {
                        transaction = ParseTrans(r);
                    }
                    if (r.NodeType != XmlNodeType.EndElement)
                    {
                        continue;
                    }
                }
                return transaction;
            }
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
        #endregion
    }
}
