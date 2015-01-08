using NgDataSync.Core.Model;

namespace NgDataSync.Core.Interfaces.Data
{
    //NOTE:
    //If you need to implement your own logic/code do it in a partial interface,
    //modifications in this file may be overwritten.
    public partial interface IAnimalRepository : IRepository<Animal>
    {
        // Add extra datainterface methods in a partial interface
    }
}