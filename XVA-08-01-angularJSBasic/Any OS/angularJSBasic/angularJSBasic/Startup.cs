using System.Web;
using angularJSBasic;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(Startup), "Start")]
namespace angularJSBasic
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