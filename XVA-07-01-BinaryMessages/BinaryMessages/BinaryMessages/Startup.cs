using System.Web;
using BinaryMessages;
using XSockets.Core.Common.Socket;

[assembly: PreApplicationStartMethod(typeof(Startup), "Start")]

namespace BinaryMessages
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