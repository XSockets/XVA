using System;

namespace PollingDbForUpdates.Core.Model
{
    public class Sales : PersistentEntity
    {
        public string Country { get; set; }
        public int Hardware { get; set; }
        public int Software { get; set; }
        public int Services { get; set; }
    }
}