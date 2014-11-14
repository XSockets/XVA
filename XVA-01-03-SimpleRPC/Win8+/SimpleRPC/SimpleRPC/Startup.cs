using Microsoft.Owin;
using Owin;
using SimpleRPC;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(Startup))]

namespace SimpleRPC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseXSockets();
        }
    }
}
