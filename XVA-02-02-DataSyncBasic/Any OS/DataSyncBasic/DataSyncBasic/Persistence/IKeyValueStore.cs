using System;
using System.Collections.Generic;

namespace DataSyncBasic.Persistence
{
    public interface IKeyValueStore<in TKey, TValue>
    {
        TValue AddOrUpdate(TKey key, TValue value);
        
        void Remove(TKey key);
        
        IEnumerable<TValue> Find(Func<TValue, bool> filter);

        TValue GetByKey(TKey key);
    }
}