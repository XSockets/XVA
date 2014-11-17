using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;

namespace angularJSBasic
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time MVC controller
    /// </summary>
    [XSocketMetadata("chat")]
    public class ChatController : XSocketController
    {
        /// <summary>
        /// This will broadcast any message to all clients
        /// connected to this controller.
        /// To use Pub/Sub replace InvokeToAll with PublishToAll
        /// </summary>
        /// <param name="message"></param>
        public void ChatMessage(IMessage message)
        {
            this.InvokeToAll(message);
        }
    }
}
