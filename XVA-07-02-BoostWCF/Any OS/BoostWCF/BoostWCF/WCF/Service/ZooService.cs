using System.ServiceModel.Activation;
using BoostWCF.WCF.Contract;
using XSockets.Core.XSocket.Helpers;

namespace BoostWCF.WCF.Service
{
    /// <summary>
    /// Showing that you can boost a existing WCF to realtime with a few lines of code.
    /// Just create a XSockets connection, and then publish messages from your WCF
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ZooService : IZooService
    {       
        
        public string Say(string message)
        {
            //Use a new instance of the ZooController to broadcast to all client connected to ZooController
            //Note that this way only works when XSockets is hosted inside teh same context (web in this case)
            new ZooController().InvokeToAll(new { message = "I was sent over websockets: " + message }, "say");

            //Do your WCF regular stuff whatever it might be... and then return
            return string.Format("I was returned from WCF: " + message);
        }
    }

}