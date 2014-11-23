using DataSyncNoSql.DataSync;
using XSockets.Plugin.Framework.Attributes;

namespace DataSyncBasic
{
    /// <summary>
    /// Implement extra methods or override datasync methods if needed
    /// </summary>
    [XSocketMetadata("SimpleList")]
    public class SimpleListController : XSocketsDataSyncController<SimpleListController>
    {

    }
}
