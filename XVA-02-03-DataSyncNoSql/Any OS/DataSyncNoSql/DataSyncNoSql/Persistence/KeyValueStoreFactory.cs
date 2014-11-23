using System.Configuration;
using OrigoDB.Core;

namespace DataSyncNoSql.Persistence
{
    public static class KeyValueStoreFactory
    {
        public static IKeyValueStore<TKey, TValue> Create<TKey, TValue>()
        {
            var persistentStore = ConfigurationManager.AppSettings["persistentStore"];
            if (persistentStore != null)
            {
                if (persistentStore.ToLower() == "true")
                    return Db.For<OrigoKeyValueStore<TKey, TValue>>();
                if (persistentStore.ToLower() == "false")
                    return new XSocketsRepositoryAdapter<TKey, TValue>();
                string msg = string.Format("Unrecognized persistentStore value [{0}], expected 'true' or 'false'", persistentStore);
                throw new ConfigurationErrorsException(msg);
            }
            return new XSocketsRepositoryAdapter<TKey, TValue>();
        }
    }
}