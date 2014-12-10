using System.ServiceModel.Activation;
using System.Web;
using BoostWCF.WCF.Contract;
using XSockets.Client40;

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
            //Only sending a string here, but anything serializable can be sent
            this.SendToAll("I was sent with xsockets: " + message, "say", "zoo");

            //Do your WCF regular stuff whatever it might be... and then return
            return string.Format("I was returned from WCF: " + message);
        }

        private void SendToAll(object obj, string topic, string controller)
        {
            //Get existing or create a new instance, then call the controller
            var location = string.Format("ws://{0}:{1}", HttpContext.Current.Request.Url.Host,HttpContext.Current.Request.Url.Port);
            ClientPool.GetInstance(location , "http://localhost").Send(obj, topic, controller);
        }
    }
}