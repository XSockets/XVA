using System.Web;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(GameSample.Startup), "Start")]
namespace GameSample
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
