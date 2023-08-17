using System;

namespace FASTsim.Library
{
    class SECSException : Exception
    {
        public SECSException(string message) : base(message)
        {
        }

        public SECSException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
