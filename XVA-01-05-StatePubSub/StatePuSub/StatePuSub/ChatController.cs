using System.Threading.Tasks;
using XSockets.Core.Common.Socket.Event.Attributes;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace StatePuSub
{
    [XSocketMetadata("chat")]
    public class ChatController : XSocketController
    {
        public Gender Gender { get; set; }
        public string City { get; set; }

        public override async Task OnOpened()
        {
            if (this.HasParameterKey("city"))
            {
                this.City = this.GetParameter("city");
            }
            if (this.HasParameterKey("gender"))
            {
                this.Gender = this.GetParameter("gender").ToEnum<Gender>();
            }
            await base.OnOpened();
        }

        /// <summary>
        /// Since we have state on the connection we do no need to pass city and gender with the text message.
        /// We can build a message on the server as well as target a subsset of subscribers with PublishTo
        /// </summary>
        /// <param name="message"></param>
        public async Task Message(string message)
        {            
            //Publish to clients in the same city and with the same gender
            await this.PublishTo(p => p.City == this.City && p.Gender == this.Gender,
                new {Message = message, City, Gender = this.Gender.ToString()}, 
                "message");
        }
    }
}
