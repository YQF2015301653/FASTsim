using System;
using System.Collections;

namespace FASTsim.Library.SECS
{
    public class Coll : IDictionary, ICollection, IEnumerable
    {
        public Hashtable m_hash = new Hashtable();
        public ArrayList m_array = new ArrayList();

        public void Add(object key, object value)
        {
            if (!ReferenceEquals(key.GetType(), typeof(string)))
            {
                throw new Exception("key must be string");
            }
            m_hash.Add(key, value);
            m_array.Add(key);
        }

        public void Insert(object key, object value)
        {
            if (!ReferenceEquals(key.GetType(), typeof(string)))
            {
                throw new Exception("key must be string");
            }
            m_hash.Add(key, value);
            m_array.Insert(0,key);
        }

        public void Insert(object key, int index, object value)
        {
            if (!ReferenceEquals(key.GetType(), typeof(string)))
            {
                throw new Exception("key must be string");
            }
            m_hash.Add(key, value);
            m_array.Insert(index, key);
        }

        public void Clear()
        {
            m_array.Clear();
            m_hash.Clear();
        }

        public bool Contains(object key) =>
            m_hash.Contains(key);

        public void CopyTo(Array array, int index)
        {
        }

        public IDictionaryEnumerator GetEnumerator() =>
            (IDictionaryEnumerator)m_hash.Values.GetEnumerator();

        public void Remove(object key)
        {
            m_array.Remove(key);
            m_hash.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            null;

        public bool IsReadOnly =>
            false;

        public object this[object key]
        {
            get =>
                !ReferenceEquals(key.GetType(), typeof(string)) ? m_hash[m_array[(int)key]] : m_hash[key];
            set
            {
            }
        }

        public ICollection Values =>
            null;

        public ICollection Keys =>
            null;

        public bool IsFixedSize =>
            false;

        public bool IsSynchronized =>
            false;

        public int Count =>
            m_hash.Count;

        public object SyncRoot =>
            null;
    }
}
