using System.Threading.Tasks;
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
        /// Same unique id for a user across instances...
        /// For example if the user has several connections you can kill them all when you want to.
        /// 
        /// Maybe you set this from a token, a cookie or just pass it in...
        /// </summary>
        public string UserId { get; set; }


        public async override Task OnOpened()
        {
            if (this.HasParameterKey("uid"))
            {
                this.UserId = this.GetParameter("uid");
            }
        }

        /// <summary>
        /// If one instance call this all instances will be disconnected
        /// </summary>
        /// <returns></returns>
        public async Task LogOut()
        {
            await this.InvokeTo(p => p.UserId == this.UserId,"die");
        }

        /// <summary>
        /// This will broadcast any message to the client being connected to the controller
        /// </summary>
        /// <param name="message"></param>
        public override async Task OnMessage(IMessage message)
        {
            await this.InvokeToAll(message);
        }
    }
}
