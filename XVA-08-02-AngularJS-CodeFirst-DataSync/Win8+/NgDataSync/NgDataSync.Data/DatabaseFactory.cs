using NgDataSync.Core.Interfaces.Data;

namespace NgDataSync.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private IDataContext _datacontext;

        public IDataContext Get()
        {
            return this._datacontext ?? (_datacontext = new DataContext());
        }

        public void Dispose()
        {
            this._datacontext.Dispose();
        }
    }
}