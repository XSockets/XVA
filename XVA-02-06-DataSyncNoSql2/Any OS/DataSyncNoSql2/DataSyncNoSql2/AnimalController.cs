using DataSyncNoSql2.DataSync;
using XSockets.Plugin.Framework.Attributes;

namespace DataSyncNoSql2
{
    [XSocketMetadata("Animal")]
    public class AnimalController : XSocketsDataSyncController<AnimalController, AnimalModel>
    {
    }
}
