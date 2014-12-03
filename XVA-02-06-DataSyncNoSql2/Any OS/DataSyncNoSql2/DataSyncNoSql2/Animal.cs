using System;
using DataSyncNoSql2.DataSync;

namespace DataSyncNoSql2
{
    /// <summary>
    /// Our basic datasync object that implements IDataSync
    /// </summary>
    [Serializable]
    public class AnimalModel : IDataSyncObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Cage { get; set; }

        public bool Hungry { get; set; }
    }
}