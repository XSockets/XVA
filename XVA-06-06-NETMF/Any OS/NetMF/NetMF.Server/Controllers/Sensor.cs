using NetMF.Server.Model;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;

namespace NetMF.Server.Controllers
{
    public class Sensor : XSocketController
    {
        /// <summary>
        /// Public accessor, can be set from any client connected to the controller.
        /// To prevent access set accessor to be NOT public or set the [NoEvent] attribute
        /// </summary>
        public Hardware Hardware { get; set; }

        /// <summary>
        /// In this case the hardware talk a very basic text protocol, so we have to extract the value from the IMessage.
        /// 
        /// The message sent out will have the topic "ChangeArduino" for example
        /// </summary>
        /// <param name="v"></param>
        public void Change(int v)
        {         
            this.InvokeTo<Monitor>(p => v <= p.Threshold, v, "Change" + Hardware);            
        }
    }
}
