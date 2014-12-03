using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;
using OrigoDB.Core.Proxy;

namespace DataSyncNoSql2.Persistence
{
    [Serializable]
    public class OrigoKeyValueStore<TKey,TValue> : Model, IKeyValueStore<TKey, TValue>
    {

        readonly SortedDictionary<TKey, TValue> _data = new SortedDictionary<TKey, TValue>();

        [Command]
        public TValue AddOrUpdate(TKey key, TValue value)
        {
            _data[key] = value;
            return value;
        }

        public void Remove(TKey key)
        {
            _data.Remove(key);
        }

        public IEnumerable<TValue> Find(Func<TValue, bool> filter)
        {
            return _data.Values.Where(filter).ToArray();
        }

        public IEnumerable<TValue> GetAll()
        {
            return _data.Values;
        }

        public TValue GetByKey(TKey key)
        {
            return _data[key];
        }
    }
}