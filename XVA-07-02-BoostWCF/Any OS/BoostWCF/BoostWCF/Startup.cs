using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using BoostWCF.WCF.Service;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(BoostWCF.Startup), "Start")]
namespace BoostWCF
{
    public static class Startup
    {
        private static IXSocketServerContainer container;
        public static void Start()
        {
            //Register the WCF-service
            RouteTable.Routes.Add(new ServiceRoute("ZooService", new WebServiceHostFactory(), typeof(ZooService)));
            //Start XSockets
            container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
            container.Start();
        }
    }
}
