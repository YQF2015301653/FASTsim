using System;
using System.Text;

namespace FASTsim.Library.SECS
{

    internal class WsEncoding
    {
        public const uint CP_UNASSIGNED = uint.MaxValue;
        private static CP_LOOKUP[] _CP_LOOKUPS = null;

        public static bool BytesToString(int CodePage, byte[] bytesIn, out string sOut)
        {
            Encoding encoding;
            sOut = null;
            try
            {
                encoding = Encoding.GetEncoding(CodePage);
            }
            catch
            {
                return false;
            }
            sOut = encoding.GetString(bytesIn);
            return true;
        }

        public static bool BytesToString(int CodePage, byte[] bytesIn, int iIndex, int iCount, out string sOut)
        {
            Encoding encoding;
            sOut = null;
            try
            {
                encoding = Encoding.GetEncoding(CodePage);
            }
            catch
            {
                return false;
            }
            sOut = encoding.GetString(bytesIn, iIndex, iCount);
            return true;
        }

        public static int DefaultCodePage() => 
            Encoding.Default.CodePage;

        public static ushort DefaultEncodingCode() => 
            ((ushort) GetEncodingCode(DefaultCodePage()));

        public static bool GetByteCount(int CodePage, string sIn, out int iByteCount)
        {
            Encoding encoding;
            iByteCount = 0;
            try
            {
                encoding = Encoding.GetEncoding(CodePage);
            }
            catch
            {
                return false;
            }
            iByteCount = encoding.GetByteCount(sIn);
            return true;
        }

        public static uint GetCodePage(int EncodingCode)
        {
            Initialize();
            for (int i = 0; i < _CP_LOOKUPS.Length; i++)
            {
                if (_CP_LOOKUPS[i].EncodingCode == EncodingCode)
                {
                    return _CP_LOOKUPS[i].CodePage;
                }
            }
            return uint.MaxValue;
        }

        public static uint GetEncodingCode(int CodePage)
        {
            Initialize();
            for (int i = 0; i < _CP_LOOKUPS.Length; i++)
            {
                if (_CP_LOOKUPS[i].CodePage == CodePage)
                {
                    return _CP_LOOKUPS[i].EncodingCode;
                }
            }
            return uint.MaxValue;
        }

        private static void Initialize()
        {
            if (_CP_LOOKUPS == null)
            {
                CP_LOOKUP[] cp_lookupArray = new CP_LOOKUP[15];
                cp_lookupArray[0] = new CP_LOOKUP(0, uint.MaxValue);
                cp_lookupArray[1] = new CP_LOOKUP(1, 0x4b0);
                cp_lookupArray[2] = new CP_LOOKUP(2, 0xfde9);
                cp_lookupArray[3] = new CP_LOOKUP(3, 0x4e9f);
                cp_lookupArray[4] = new CP_LOOKUP(4, 0x4e4);
                cp_lookupArray[5] = new CP_LOOKUP(5, 0x36a);
                cp_lookupArray[6] = new CP_LOOKUP(6, 0x36a);
                cp_lookupArray[7] = new CP_LOOKUP(7, uint.MaxValue);
                cp_lookupArray[8] = new CP_LOOKUP(8, 0x3a4);
                cp_lookupArray[9] = new CP_LOOKUP(9, 0xcadc);
                cp_lookupArray[10] = new CP_LOOKUP(10, 0xcaed);
                cp_lookupArray[11] = new CP_LOOKUP(11, 0x3a8);
                cp_lookupArray[12] = new CP_LOOKUP(12, 0xcae0);
                cp_lookupArray[13] = new CP_LOOKUP(13, 950);
                cp_lookupArray[14] = new CP_LOOKUP(14, uint.MaxValue);
                _CP_LOOKUPS = cp_lookupArray;
            }
        }

        public static bool IsValidCodePage(int CodePage)
        {
            bool flag;
            try
            {
                Encoding.GetEncoding(CodePage);
                flag = true;
            }
            catch (Exception)
            {
                return false;
            }
            return flag;
        }

        public static bool StringToBytes(int CodePage, string sIn, int iByteIndex, ref byte[] bytesOut, ref int iLength)
        {
            Encoding encoding;
            iLength = 0;
            try
            {
                encoding = Encoding.GetEncoding(CodePage);
            }
            catch
            {
                return false;
            }
            iLength = encoding.GetBytes(sIn, 0, sIn.Length, bytesOut, iByteIndex);
            return (iLength > 0);
        }
    }
}

