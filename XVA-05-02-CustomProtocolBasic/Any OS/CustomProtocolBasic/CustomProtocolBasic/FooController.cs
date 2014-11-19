using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;

namespace CustomProtocolBasic
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time controller
    /// </summary>
    [XSocketMetadata("Foo")]
    public class FooController : XSocketController
    {
        /// <summary>
        /// We could have used IMessage instead of string message, that would give us details about
        /// controller, topic, data and messagetype etc
        /// </summary>
        /// <param name="message"></param>
        public void Bar(string message)
        {
            //Will broadcast to all clients regardless of subscription
            //this.InvokeToAll(message,"bar");

            //Send to subscribers only
            this.PublishToAll(message, "bar");
        }
    }
}
