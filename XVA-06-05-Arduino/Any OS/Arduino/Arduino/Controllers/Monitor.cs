using Arduino.Model;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;

namespace Arduino.Controllers
{
    /// <summary>
    /// Clients monitoring sensors connect to this controller.
    /// </summary>
    public class Monitor : XSocketController
    {
        /// <summary>
        /// Individual sensor level
        /// </summary>
        public int Threshold { get; set; }

        public override void OnOpened()
        {
            if (this.HasParameterKey("threshold"))            
                this.Threshold = int.Parse(this.GetParameter("threshold"));            
        }

        /// <summary>
        /// Change the threshold for a specific hardware
        /// </summary>
        public void SetThreshold(HardwareSettings hws)
        {
            //Set new hardware limit
            this.InvokeTo<Sensor>(p => p.Hardware == hws.Hardware, hws.Threshold, "threshold");

            //Tell all monitoring clients about the change
            this.InvokeToAll<Monitor>(hws.Threshold, "threshold" + hws.Hardware);
        }
    }
}