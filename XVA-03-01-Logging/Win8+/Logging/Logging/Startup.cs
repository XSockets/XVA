using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(Logging.Startup))]

namespace Logging
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseXSockets();
        }
    }
}
