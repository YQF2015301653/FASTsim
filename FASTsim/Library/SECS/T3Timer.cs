namespace FASTsim.Library.SECS
{
    internal class T3Timer
    : System.Timers.Timer
    {

        private uint m_TransactionId;

        public uint TransactionId
        {
            get
            {
                return m_TransactionId;
            }
        }

        internal T3Timer(uint tid)
        {
            m_TransactionId = tid;
        }
    }
}
