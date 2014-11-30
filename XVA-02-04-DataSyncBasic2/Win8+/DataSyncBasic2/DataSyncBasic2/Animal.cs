using System;
using DataSyncBasic2.DataSync;

namespace DataSyncBasic2
{
    /// <summary>
    /// Our basic datasync object that implements IDataSync
    /// </summary>
    public class AnimalModel : IDataSyncObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Cage { get; set; }

        public bool Hungry { get; set; }
    }
}