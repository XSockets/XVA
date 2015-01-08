using NgDataSync.Core.Model;
using NgDataSync.Core.ViewModel;

namespace NgDataSync.Core.Interfaces.Service
{
    //NOTE:
    //If you need to implement your own logic/code do it in a partial interface,
    //modifications in this file may be overwritten.
    public partial interface IAnimalService : IService<Animal, AnimalViewModel>
    {
        // Add extra serviceinterface methods in a partial interface
    }
}