using GameSample;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(Startup))]

namespace GameSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseXSockets();
        }
    }
}