using DataSyncBasic2.DataSync;
using XSockets.Plugin.Framework.Attributes;

namespace DataSyncBasic2
{
    [XSocketMetadata("Animal")]
    public class AnimalController : XSocketsDataSyncController<AnimalController, AnimalModel>
    {
    }
}
