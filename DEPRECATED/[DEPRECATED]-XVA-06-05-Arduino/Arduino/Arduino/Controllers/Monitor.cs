using Arduino.Model;
using System.Threading.Tasks;
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

        public override async Task OnOpened()
        {
            if (this.HasParameterKey("threshold"))            
                this.Threshold = int.Parse(this.GetParameter("threshold"));
            await base.OnOpened();           
        }

        /// <summary>
        /// Change the threshold for a specific hardware
        /// </summary>
        public async Task SetThreshold(HardwareSettings hws)
        {
            //Set new hardware limit
            await this.InvokeTo<Sensor>(p => p.Hardware == hws.Hardware, hws.Threshold, "threshold");

            //Tell all monitoring clients about the change
            await this.InvokeToAll<Monitor>(hws.Threshold, "threshold" + hws.Hardware);
        }
    }
}