using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using System.Threading.Tasks;

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
        public override async Task OnMessage(IMessage message)
        {
            await this.InvokeToAll(message);
        }

        //public async Task Say(string message)
        //{
        //    await this.InvokeToAll(message, "say");
        //}
    }
}
