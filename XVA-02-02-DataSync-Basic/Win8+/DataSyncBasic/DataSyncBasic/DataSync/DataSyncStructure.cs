using System;

namespace DataSyncBasic.DataSync
{
    /// <summary>
    /// Used for passing around objects to synchronize
    /// </summary>
    public class DataSyncStructure
    {
        /// <summary>
        /// Identifier for the unique object
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Identifier for the topic
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// The actual object (anything serializable)
        /// </summary>
        public object Object { get; set; }
    }
}