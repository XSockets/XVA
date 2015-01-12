using NgDataSync.Core.Model;
using NgDataSync.Core.ViewModel;
using NgDataSync.Service;
using XSockets.Plugin.Framework.Attributes;

namespace NgDataSync.XSocketsModules
{
    [XSocketMetadata("Animal")]
    public class AnimalController : XSocketsDataSyncController<AnimalController, Animal, AnimalViewModel, AnimalService>
    {
    }
}