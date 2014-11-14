using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace SimpleRPC
{
    [XSocketMetadata("chat")]
    public class ChatController : XSocketController
    {
        /// <summary>
        /// This will broadcast any message to the client being connected to the controller
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(IMessage message)
        {
            this.InvokeToAll(message);
        }
    }
}
