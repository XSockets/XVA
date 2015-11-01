using System.Web;
using XSockets.Core.Common.Socket;
using XSockets.Logger;
using Serilog;

[assembly: PreApplicationStartMethod(typeof(SimpleRPC.Startup), "Start")]
namespace SimpleRPC
{
    public class MyLogger : XLogger
    {
        public MyLogger()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Trace().CreateLogger();
        }
    }
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
