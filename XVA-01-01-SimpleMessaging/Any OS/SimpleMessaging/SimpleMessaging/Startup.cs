using XSockets.Core.Common.Socket;

[assembly: System.Web.PreApplicationStartMethod(typeof(SimpleMessaging.Startup), "Start")]
namespace SimpleMessaging
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
