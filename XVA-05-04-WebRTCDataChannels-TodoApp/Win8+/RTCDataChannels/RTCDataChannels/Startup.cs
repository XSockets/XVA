using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(RTCDataChannels.Startup))]

namespace RTCDataChannels
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseXSockets();
        }
    }
}
