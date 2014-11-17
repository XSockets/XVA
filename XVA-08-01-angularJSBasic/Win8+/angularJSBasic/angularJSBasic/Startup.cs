using angularJSBasic;
using Microsoft.Owin;
using Owin;
using XSockets.Owin.Host;

[assembly: OwinStartup(typeof(Startup))]
namespace angularJSBasic
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseXSockets();
        }
    }
}