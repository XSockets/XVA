using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;

namespace BoostWCF
{
    [XSocketMetadata("Zoo")]
    public class ZooController : XSocketController
    {
        /// <summary>
        /// This "generic" method will accept any message and broadcast it to all clients
        /// connected to the controller.
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(IMessage message)
        {
            this.InvokeToAll(message);
        }

        //public void Say(string message)
        //{
        //    this.InvokeToAll(message, "say");
        //}
    }
}