using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using XSockets.Core.Common.Socket;
using XSockets.Plugin.Framework;

[assembly: OwinStartup(typeof(NgDataSync.Startup))]

namespace NgDataSync
{
    public class Startup
    {
        private static IXSocketServerContainer container;
    
      
        public void Configuration(IAppBuilder app)
        {
            container = Composable.GetExport<IXSocketServerContainer>();
            container.Start();              
        }
    }
}
