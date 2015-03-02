using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using RTCDataChannels.RealtimeControllers.Constants;
using RTCDataChannels.RealtimeControllers.Models;
using XSockets.Core.Common.Socket.Event.Attributes;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;

namespace RTCDataChannels.RealtimeControllers
{ 
    /// <summary>
    /// A custom Peerbroker for WebRTC signaling and WebSocket communication on top of XSockets.NET
    /// </summary>
    public class ConnectionBroker : XSocketController, IConnectionBroker
    {
        public void GetIceServer(Guid ctx)
        {
            string responseFromServer = null;
            try
            {
            var request =
                         WebRequest.Create(@"https://computeengineondemand.appspot.com/turn?username=34096653&key=4080218913");
            request.Method = "GET";
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse ();
            var dataStream = response.GetResponseStream ();
            var reader = new StreamReader (dataStream);
                responseFromServer = reader.ReadToEnd();
            reader.Close ();
            response.Close ();
            }
            catch (Exception ex){
               this.InvokeError(ex);
            }
               
                  this.Invoke(new  { iceServers = responseFromServer, ctx },"iceServers");
                  
            }
          
        

        #region Public Properties

        /// <summary>
        /// List of PeerConnections that the Peer has connected to
        /// </summary>
        [NoEvent]
        public List<IPeerConnection> Connections { get; set; }

        /// <summary>
        /// The Peer for this client
        /// </summary>
        [NoEvent]
        public IPeerConnection Peer { get; set; }

        [NoEvent]
        public IPresence Presence { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor - setting up connectionlist and open/close events
        /// </summary>
        public ConnectionBroker()
        {
            Connections = new List<IPeerConnection>();
        }

        #endregion

        #region Private Methods & Events
        /// <summary>
        /// When a client connects create a new PeerConnection and send the information the the client
        /// </summary>
        public override void OnOpened()
        {
            IPresence user = new Presence {Online = true, UserName = "Unknown", Id = this.PersistentId};
            //Update user
            if (XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.ContainsKey(this.PersistentId))
            {
                user = XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.GetById(user.Id);                
                if (this.HasParameterKey("username"))
                    user.UserName = this.GetParameter("username");                               
            }
            SavePresence(user);
            var others = XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.Find(p => p.Id != user.Id);
            Composable.GetExport<IXLogger>().Information("Others {@a}",others);
            this.Invoke(others,"allusers");

            // Get the context from a parameter if it exists
            var context = Guid.NewGuid();

            if (this.HasParameterKey("ctx"))
            {
                var p = this.GetParameter("ctx");
                context = Guid.Parse(p);
            }

            Peer = new PeerConnection
            {
                Context = context,
                PeerId = ConnectionId
            };

            this.Invoke(Peer, Events.Context.Created);                        
        }

        public void GetContext()
        {
            this.Invoke(Peer, Events.Context.Created);
        }

        /// <summary>
        /// When a client disconnects tell the other clients about the Peer being lost
        /// </summary>
        public override void OnClosed()
        {
            this.NotifyPeerLost();
            Thread.Sleep(1000);
            //Update user
            var user = XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.GetById(this.PersistentId);
            user.Online = false;
            SavePresence(user);
        }

        private void NotifyPeerLost()
        {
            if (Peer == null) return;
            this.InvokeTo(f => f.Peer.Context == Peer.Context, Peer, Events.Peer.Lost);
        }

        #endregion

        #region Overrides from XSocketController

        #endregion

        #region Signaling Methods

        /// <summary>
        /// Distribute signals (SDP's)
        /// </summary>
        /// <param name="signalingModel"></param>
        public void ContextSignal(SignalingModel signalingModel)
        {
            this.InvokeTo<ConnectionBroker>(f => f.ConnectionId == signalingModel.Recipient, signalingModel, Events.Context.Signal);
        }

        public void ConnectToContext()
        {
            // Pass the client a list of Peers to Connect
            this.Invoke(this.GetConnections(this.Peer)
                       .Where(q => !q.Connections.Contains(this.Peer)).
                        Select(p => p.Peer).AsMessage(Events.Context.Connect));
        }

        /// <summary>
        /// Leave a context
        /// </summary>
        public void LeaveContext()
        {
            this.NotifyPeerLost();

            this.Peer.Context = new Guid();
            this.Invoke(Peer, Events.Context.Created);
        }

        public void OfferContext(string a)
        {
            //var p = new {Peer = this.Peer};
            var users =
                this.FindOn<ConnectionBroker>(
                    u => u.Peer.Context == this.Peer.Context && u.PersistentId != this.PersistentId);

            foreach (var user in users)
            {
                user.Invoke(this.Peer, Events.Context.Offer);
            }
        }

        /// <summary>
        ///    Current client changes context
        /// </summary>
        /// <param name="context"></param>
        public void ChangeContext(Guid context)
        {
            this.Peer.Context = context;
            this.NotifyContextChange(context, this.ConnectToContext);
        }


        public void SetContext(Guid context)
        {
            this.Peer.Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient"></param>
        public void DisconnectPeer(Guid recipient)
        {
            this.PublishTo(p => p.ConnectionId == recipient, new { Sender = this.ConnectionId }, Events.Peer.Disconnect);
        }

        #endregion

        #region Stream Methods

        /// <summary>
        /// Notify PeerConnections on the current context that a MediaStream is removed.
        /// </summary>
        /// <param name="streamId"></param>
        public void RemoveStream(string streamId)
        {
            this.InvokeTo<ConnectionBroker>(f => f.Peer.Context == Peer.Context, new StreamModel { Sender = ConnectionId, StreamId = streamId }, Events.Stream.Remove);
        }

        /// <summary>
        /// Notify PeerConnections on the current context that a MediaStream is added.
        /// </summary>
        /// <param name="streamId"></param>
        /// <param name="description">JSON</param>
        public void AddStream(string streamId, string description)
        {
            this.InvokeTo<ConnectionBroker>(f => f.Peer.Context == Peer.Context,
                new StreamModel
                {
                    Sender = ConnectionId,
                    StreamId = streamId,
                    Description = description
                }, Events.Stream.Add);
        }
        
        private IEnumerable<ConnectionBroker> GetConnections(IPeerConnection peerConnection)
        {
            return this.Find(f => f.Peer.Context == peerConnection.Context).Select(p => p).Except(new List<ConnectionBroker> { this });
        }

        private void NotifyContextChange(Guid context, Action callback)
        {
            this.InvokeTo<ConnectionBroker>(c => c.Peer.Context == context, this.Find(q => q.Peer.Context == context).Select(p => p.Peer), Events.Context.Changed);
            if (callback != null)
                callback();
        }
        #endregion

        #region Presence Methods

        public void SetUsername(string username)
        {
            var user = XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.GetById(this.PersistentId);
            user.UserName = username;
            SavePresence(user);            
        }

        public void SetAvailability(Availability availability)
        {
            var user = XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.GetById(this.PersistentId);
            user.Availability = availability;
            SavePresence(user);            
        }

        private void SavePresence(IPresence presence)
        {
            var user = XSockets.Core.Utility.Storage.Repository<Guid, IPresence>.AddOrUpdate(this.PersistentId, presence);
            this.InvokeToOthers(user, "userupdate");
        }
        #endregion

        #region VoiceMessage Methods
        public Guid SaveVoiceMessage(IMessage message)
        {
            var voiceMessage = message.Extract<VoiceMessage>();
            voiceMessage.Sender = this.PersistentId;   // Mark this peer as sender
            voiceMessage.Bytes = message.Blob.ToArray();
            XSockets.Core.Utility.Storage.Repository<Guid, IVoiceMessage>.AddOrUpdate(voiceMessage.Id, voiceMessage);
            return voiceMessage.Id;
        }

        public void GetVoiceMessage(Guid id)
        {
            var voiceMessage = XSockets.Core.Utility.Storage.Repository<Guid, IVoiceMessage>.GetById(id);

            this.Invoke(voiceMessage.Bytes.ToArray(), new {voiceMessageId = voiceMessage.Id},"voicemessage");
        }
        public object CheckVoiceMessages()
        {
            var voiceMessages = XSockets.Core.Utility.Storage.Repository<Guid, IVoiceMessage>
                .Find(q => q.Recipient == this.PersistentId)
                .Select(m => new {m.Created, m.Id, m.Sender,Size = m.Bytes.Count()});
            return voiceMessages;
        } 
#endregion 

    }
}