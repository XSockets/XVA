using System;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using System.Web.Routing;
using BoostWCF.WCF.Service;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(BoostWCF.Startup))]

namespace BoostWCF
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Register the WCF-service
            RouteTable.Routes.Add(new ServiceRoute("ZooService", new WebServiceHostFactory(), typeof(ZooService)));

            app.UseXSockets();
        }
    }
}
