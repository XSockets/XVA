using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using System.Threading.Tasks;

namespace SimpleMessaging
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time controller
    /// </summary>
    public class Chat : XSocketController
    {
        /// <summary>
        /// This will broadcast any message to all clients
        /// connected to this controller.
        /// To use Pub/Sub replace InvokeToAll with PublishToAll
        /// </summary>
        /// <param name="message"></param>
        public override async Task OnMessage(IMessage message)
        {
            //message.QoS = XSockets.Core.Common.Socket.Event.Arguments.QoS.AtLeastOnce;
            await this.InvokeToAll(message);
        }
    }
}
