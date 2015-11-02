using System;

//IMPORTANT - Modifications to this file may be overwritten:
//If you need to implement your own logic/code do it in a partial class/interface.
namespace PollingDbForUpdates.Core.Interfaces.Data
{
    public interface IDatabaseFactory : IDisposable
    {
        IDataContext Get();
    }
}