using System;
using System.Collections.Generic;
using XSockets.Core.Utility.Storage;

namespace DataSyncBasic.Persistence
{
    public class XSocketsRepositoryAdapter<TKey, TValue> : IKeyValueStore<TKey, TValue>
    {

        public TValue AddOrUpdate(TKey key, TValue value)
        {
            return Repository<TKey, TValue>.AddOrUpdate(key, value);
        }

        public void Remove(TKey key)
        {
            Repository<TKey, TValue>.Remove(key);
        }

        public IEnumerable<TValue> Find(Func<TValue, bool> filter)
        {
            return Repository<TKey, TValue>.Find(filter);
        }

        public TValue GetByKey(TKey key)
        {
            return Repository<TKey, TValue>.GetById(key);
        }
    }
}