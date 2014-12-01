using Arduino.Model;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;

namespace Arduino.Controllers
{
    /// <summary>
    /// Hardware connects to this controller
    /// </summary>
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
        /// The message sent out will haev the topic "ChangeArduino" for example
        /// </summary>
        /// <param name="message"></param>
        public void Change(IMessage message)
        {
            try
            {
                var v = message.Extract<int>();
                this.InvokeTo<Monitor>(p => v <= p.Threshold, v, "Change" + Hardware);
            }
            catch { }
        }
    }
}
