using System.Web;
using XSockets.Core.Common.Socket;
using XSocketsFallback;

[assembly: PreApplicationStartMethod(typeof(Startup), "Start")]
namespace XSocketsFallback
{
    public static class Startup
    {
        private static IXSocketServerContainer container;
        public static void Start()
        {
            container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
            container.Start();
        }
    }
}
