using System.Web;
using AudioStreamer;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(Startup), "Start")]
namespace AudioStreamer
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