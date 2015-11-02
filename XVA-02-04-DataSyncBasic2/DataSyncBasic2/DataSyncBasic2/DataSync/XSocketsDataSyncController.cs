using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XSockets.Core.Common.Socket;
using XSockets.Core.Utility.Storage;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;

namespace DataSyncBasic2.DataSync
{
    /// <summary>
    /// XSockets sample controller for basic DataSync operations.
    /// 
    /// Replace the Repository with your own service layer, ORM or whatever to have another persistence store.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public abstract class XSocketsDataSyncController<T,TV> : XSocketController
        where T : class, IXSocketController
        where TV : class, IDataSyncObject
    {
        private T _controller;
        private readonly string _topic = typeof (TV).Name;

        /// <summary>
        /// To get the correct controller type, cant use abstract class when sending data
        /// </summary>
        private T Controller
        {
            get { return _controller ?? (_controller = (T)Composable.GetExport<IXSocketController>(typeof(T))); }
        }

        /// <summary>
        /// Will fetch and send all data in the repository for each topic sent in with the connection.
        /// </summary>
        public override async Task OnOpened()
        {
            //Send back the data in the repo for the type TV
            await this.Invoke(Repository<Guid, TV>.GetAll(), string.Format("{0}:{1}", DataSyncCommand.Init, _topic)); 
        }        

        /// <summary>
        /// Search the repository, not exposed to clients since accessor is protected
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TV> Find(Func<TV, bool> expression)
        {
            return Repository<Guid, TV>.Find(expression);
        } 

        /// <summary>
        /// Adds/Updates the data from in reposiotry and tells all (subscribing) clients about it.
        /// 
        /// Override to implement custom logic
        /// </summary>
        /// <param name="model"></param>
        public virtual async Task Update(TV model)
        {
            var command = DataSyncCommand.Update;
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                command = DataSyncCommand.Add;
            }

            model = Repository<Guid, TV>.AddOrUpdate(model.Id, model);
            await Sync(command, model);
        }

        /// <summary>
        /// Deletes the data from the reposiotry and tells all (subscribing) clients about it
        /// 
        /// Override to implement custom logic
        /// </summary>
        /// <param name="model"></param>
        public virtual async Task Delete(TV model)
        {
            model = Repository<Guid, TV>.GetById(model.Id);
            if (model != null)
            {
                Repository<Guid, TV>.Remove(model.Id);
                await Sync(DataSyncCommand.Delete, model);
            }
        }

        /// <summary>
        /// Will do a PUBLISH of changes by default, override to implement specific logic and/or RPC
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        protected virtual async Task Sync(string command, TV model)
        {
            await Controller.PublishToAll(model, string.Format("{0}:{1}", command, _topic));
        }
    }
}