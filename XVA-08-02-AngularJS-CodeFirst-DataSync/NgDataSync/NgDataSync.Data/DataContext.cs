using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using NgDataSync.Core.Model;
using NgDataSync.Core.Interfaces.Data;

namespace NgDataSync.Data
{
    public class DataContext : DbContext, IDataContext
    {
        //If you do not use the scaffolders, register your domainmodel entities here.
        //public DbSet<T> EntityNames { get; set; }        

        public DataContext()
            : this(true)
        {
        }

        public DataContext(bool proxyCreation = true)
        {
            this.Configuration.ProxyCreationEnabled = proxyCreation;

            //[DropAndReCreate if in debug and model is changed. ONLY FOR DEVELOPMENT!!!]			
            if (System.Diagnostics.Debugger.IsAttached)
                Database.SetInitializer(new DataSeeder());
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="modelBuilder"></param>
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //Implement custom setup here... ColumnNames, Validation, Relations etc.
        //    base.OnModelCreating(modelBuilder);
        //}

        public ObjectContext ObjectContext()
        {
            return ((IObjectContextAdapter)this).ObjectContext;
        }

        public virtual IDbSet<T> DbSet<T>() where T : PersistentEntity
        {
            return Set<T>();
        }

        public new DbEntityEntry Entry<T>(T entity) where T : PersistentEntity
        {
            return base.Entry(entity);
        }

        public DbSet<Animal> Animals { get; set; }
    }

    public class DataSeeder : DropCreateDatabaseIfModelChanges<DataContext>
    {
        //protected override void Seed(DataContext context)
        //{            
        //Insert data to your context here to add data when the database is created!
        //}       
    }
}