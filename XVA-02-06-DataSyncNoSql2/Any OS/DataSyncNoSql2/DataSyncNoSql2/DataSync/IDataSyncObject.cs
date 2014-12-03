using System;

namespace DataSyncNoSql2.DataSync
{
    /// <summary>
    /// DataSync interface to have an ID on all data sync objects
    /// </summary>
    public interface IDataSyncObject
    {
        Guid Id { get; set; }
    }
}
