using XSockets.Plugin.Framework.Attributes;
using NgDataSync.Core.Model;
using NgDataSync.Core.ViewModel;
using NgDataSync.Service;
using NgDataSync.XSocketsModules;

namespace NgDataSync
{
    [XSocketMetadata("Animal")]
    public class AnimalController : XSocketsDataSyncController<AnimalController, Animal, AnimalViewModel, AnimalService>
    {
    }
}