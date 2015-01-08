//IMPORTANT - Modifications to this file may be overwritten:
//If you need to implement your own logic/code do it in a partial class/interface.
using System;
namespace NgDataSync.Core.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}