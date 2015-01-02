using Ninject.Modules;
using PollingDbForUpdates.Core.Interfaces.Data;
using PollingDbForUpdates.Core.Interfaces.Service;
using PollingDbForUpdates.Data;
using PollingDbForUpdates.Service;

namespace PollingDbForUpdates.NinjectModules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().InThreadScope();
            Bind<IDatabaseFactory>().To<DatabaseFactory>().InThreadScope();
            Bind<ISalesRepository>().To<SalesRepository>().InThreadScope();
            Bind<ISalesService>().To<SalesService>().InThreadScope();
        }
    }

}
