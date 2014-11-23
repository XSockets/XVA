using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataSyncBasic.DataSync
{



    /// <summary>
    /// Used for passing around objects to synchronize
    /// </summary>
    [Serializable]
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

        private string _json;

        public object Object {
            get { return _json != null ? JObject.Parse(_json) : null; }
            set { _json = value != null ? ((JObject) value).ToString(Formatting.None) : null; } 
        }        
    }
}