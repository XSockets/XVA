using Microsoft.Owin;
using Owin;
using RTCDataChannels;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(Startup))]

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