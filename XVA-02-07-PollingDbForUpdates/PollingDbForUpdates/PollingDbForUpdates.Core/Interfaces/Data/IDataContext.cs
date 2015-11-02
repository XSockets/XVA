using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using PollingDbForUpdates.Core.Model;
	
//IMPORTANT - Modifications to this file may be overwritten:
//If you need to implement your own logic/code do it in a partial class/interface.
namespace PollingDbForUpdates.Core.Interfaces.Data
{
    public interface IDataContext
    {
        ObjectContext ObjectContext();
        IDbSet<T> DbSet<T>() where T : PersistentEntity;
        DbEntityEntry Entry<T>(T entity) where T : PersistentEntity;
        void Dispose();
    }
}