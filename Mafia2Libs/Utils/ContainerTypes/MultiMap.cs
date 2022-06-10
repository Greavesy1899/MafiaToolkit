using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils.ContainerTypes
{
    public class MultiMap<TKey, TValue> : IDictionary<TKey, List<TValue>>, IEnumerable<KeyValuePair<TKey, List<TValue>>>
    {
        private Dictionary<TKey, List<TValue>> Container;

        public MultiMap()
        {
            Container = new Dictionary<TKey, List<TValue>>();
        }

        public List<TValue> this[TKey key] { get => Container[key]; set => Container[key] = value; }

        public ICollection<TKey> Keys => Container.Keys;

        public ICollection<List<TValue>> Values => Container.Values;

        public int Count => Container.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, List<TValue>>>)Container).IsReadOnly;

        public void Add(TKey key, List<TValue> value)
        {
            Container.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, List<TValue>> item)
        {
            Container.Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (Container.ContainsKey(key))
            {
                Container[key].Add(value);
            }
            else
            {
                List<TValue> NewList = new List<TValue>();
                NewList.Add(value);
                Container.Add(key, NewList);
            }
        }

        public void AddKey(TKey Key)
        {
            if(ContainsKey(Key) == false)
            {
                Add(Key, new List<TValue>());
            }
        }

        public void Clear()
        {
            Container.Clear();
        }

        public bool Contains(KeyValuePair<TKey, List<TValue>> item)
        {
            return Container.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return Container.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, List<TValue>>>)Container).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            return Container.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return Container.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, List<TValue>> item)
        {
            return Container.Remove(item.Key);
        }

        /***
         * Remove the Value from the specified Key.
         * @param Key - Key of the list the value is expected to be in
         * @param Value - The Value we want to delete from the list.
         * @return bool - Did we remove the Value from the specified Key?
         */
        public bool Remove(TKey Key, TValue Value)
        {
            if(Container.ContainsKey(Key))
            {
                List<TValue> FoundList = Container[Key];
                return FoundList.Remove(Value);
            }

            return false;
        }

        public bool TryGetValue(TKey key, out List<TValue> value)
        {
            return Container.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Container).GetEnumerator();
        }
    }
}
