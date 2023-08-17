using System.Runtime.InteropServices;

namespace FASTsim.Library.SECS
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct CP_LOOKUP
    {
        public uint EncodingCode;
        public uint CodePage;
        public CP_LOOKUP(uint iEncodingCode, uint iCodePage)
        {
            EncodingCode = iEncodingCode;
            CodePage = iCodePage;
        }
    }
}

