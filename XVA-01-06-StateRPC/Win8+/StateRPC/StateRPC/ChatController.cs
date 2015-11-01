using System.Threading.Tasks;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace StateRPC
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
        }

        /// <summary>
        /// Since we have state on the connection we do no need to pass city and gender with the text message.
        /// We can build a message on the server as well as target a subsset of clients with InvokeTo
        /// </summary>
        /// <param name="message"></param>
        public void Message(string message)
        {
            //Send to clients in the same city and with the same gender
            this.InvokeTo(p => p.City == this.City && p.Gender == this.Gender, new { Message = message, City, Gender = this.Gender.ToString() }, "message");
        }
    }
}