using System.Web;
using DataSyncNoSql2;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(Startup), "Start")]
namespace DataSyncNoSql2
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
