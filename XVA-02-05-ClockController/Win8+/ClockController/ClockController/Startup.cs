using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(ClockController.Startup))]

namespace ClockController
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseXSockets();
        }
    }
}
