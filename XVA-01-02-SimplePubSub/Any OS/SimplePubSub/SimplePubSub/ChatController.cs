using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;

namespace SimplePubSub
{
    [XSocketMetadata("chat")]
    public class ChatController : XSocketController
    {
        /// <summary>
        /// This will publish any message to the client being subscribers of the "Topic" part of the IMessage
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(IMessage message)
        {            
            this.PublishToAll(message);
        }        
    }
}
