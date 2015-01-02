using System.Diagnostics;
using Microsoft.Owin;
using Owin;
using XSockets.Core.Common.Socket;
using XSockets.Plugin.Framework;

[assembly: OwinStartupAttribute(typeof(PollingDbForUpdates.Startup))]
namespace PollingDbForUpdates
{
    public partial class Startup
    {
        private static IXSocketServerContainer container;
        public void Configuration(IAppBuilder app)
        {                        
            container = Composable.GetExport<IXSocketServerContainer>();
            container.Start();
        }
    }
}
