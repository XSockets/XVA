using System;
using System.Collections.Generic;
using DataSyncBasic.Persistence;
using XSockets.Core.Common.Socket;
using XSockets.Core.Utility.Storage;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;

namespace DataSyncBasic.DataSync
{
    /// <summary>
    /// XSockets sample controller for basic DataSync operations.
    /// 
    /// Replace the Repository with your own service layer to get fine grained control over what to load.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class XSocketsDataSyncController<T> : XSocketController where T : class, IXSocketController
    {
        private T _controller;

        readonly IKeyValueStore<Guid, DataSyncStructure> _store 
            = KeyValueStoreFactory.Create<Guid, DataSyncStructure>();

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
        public override void OnOpened()
        {
            //Get all data for each topic passed in
            if (this.HasParameterKey("topics"))
            {
                foreach (var topic in this.GetParameter("topics").Split(','))
                {
                    var persistentData = _store.Find(p => p.Topic == topic);
                    InitalData(persistentData, topic);
                }
            }
        }

        /// <summary>
        /// Passes the persisted data to the newly connected client.
        /// </summary>
        /// <param name="persistentData"></param>
        /// <param name="topic"></param>
        protected virtual void InitalData(IEnumerable<DataSyncStructure> persistentData, string topic)
        {
            //We can use this in here since we use the socket directly with invoke.
            this.Invoke(persistentData, string.Format("{0}:{1}", DataSyncCommand.Init, topic));
        }

        /// <summary>
        /// Adds/Updates the data structure from in reposiotry and tells all (subscribing) clients about it.
        /// 
        /// Override to implement custom logic
        /// </summary>
        /// <param name="model"></param>
        public virtual void Update(DataSyncStructure model)
        {
            var command = DataSyncCommand.Update;
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                command = DataSyncCommand.Add;
            }

            model = _store.AddOrUpdate(model.Id, model);
            Sync(command, model);
        }

        /// <summary>
        /// Deletes the data structure from the reposiotry and tells all (subscribing) clients about it
        /// 
        /// Override to implement custom logic
        /// </summary>
        /// <param name="model"></param>
        public virtual void Delete(DataSyncStructure model)
        {
            model = _store.GetByKey(model.Id);
            if (model != null)
            {
                _store.Remove(model.Id);
                Sync(DataSyncCommand.Delete, model);
            }
        }

        /// <summary>
        /// Will do a PUBLISH of changes by default, override to implement specific logic and/or RPC
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        protected virtual void Sync(string command, DataSyncStructure model)
        {
            Controller.PublishToAll(model, string.Format("{0}:{1}",command,model.Topic));
        }
    }
}
