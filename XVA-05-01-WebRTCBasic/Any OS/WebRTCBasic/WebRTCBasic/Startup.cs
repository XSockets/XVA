using System.Web;
using WebRTCBasic;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(Startup), "Start")]
namespace WebRTCBasic
{
    public static class Startup
    {
        private static IXSocketServerContainer _container;
        public static void Start()
        {
            _container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
            _container.Start();
        }
    }
}
