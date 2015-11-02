using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using PollingDbForUpdates.Core.Model;
using PollingDbForUpdates.Core.Interfaces.Data;
using System;

namespace PollingDbForUpdates.Data
{
    public class DataContext : DbContext, IDataContext
    {
        //If you do not use the scaffolders, register your domainmodel entities here.
        public DbSet<Sales> Sales { get; set; }        

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
    }
    
    public class DataSeeder : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var d = DateTime.Now;
            context.Sales.Add(new Sales
            {
                Country = "USA",
                Hardware = 234,
                Services = 21,
                Software = 45,
                Created = d.ToString(),
                Updated = d.ToString()
            });

            context.Sales.Add(new Sales
            {
                Country = "SWE",
                Hardware = 134,
                Services = 55,
                Software = 40,
                Created = d.ToString(),
                Updated = d.ToString()
            });

            context.Sales.Add(new Sales
            {
                Country = "ENG",
                Hardware = 340,
                Services = 89,
                Software = 14,
                Created = d.ToString(),
                Updated = d.ToString()
            });

            context.SaveChanges();
        }
    }
}