using XSockets.Core.Common.Socket;

[assembly: System.Web.PreApplicationStartMethod(typeof(SimplePubSub.Startup), "Start")]
namespace SimplePubSub
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
