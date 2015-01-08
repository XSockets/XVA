
using Ninject.Modules;
using NgDataSync.Core.Interfaces.Data;
using NgDataSync.Core.Interfaces.Service;
using NgDataSync.Data;
using NgDataSync.Service;

namespace NgDataSync.NinjectModules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
			Bind<IUnitOfWork>().To<UnitOfWork>().InThreadScope();	
			Bind<IDatabaseFactory>().To<DatabaseFactory>().InThreadScope();	
			Bind<IAnimalRepository>().To<AnimalRepository>().InThreadScope();	
			Bind<IAnimalService>().To<AnimalService>().InThreadScope();	
            
        }
    }    
}