using NgDataSync.Core.Model;
using NgDataSync.Core.Interfaces.Data;

namespace NgDataSync.Data
{
    //NOTE:
    //If you need to implement your own logic/code do it in a partial class,
    //modifications in this file may be overwritten.
    public partial class AnimalRepository : BaseRepository<Animal>, IAnimalRepository
    {
        public AnimalRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
        //Implement custom code in a partial class        
    }
}