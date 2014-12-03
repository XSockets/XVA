using System;
using XSockets.Core.Common.Socket;

namespace NetMF.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>())
            {
                container.Start();
                Console.ReadLine();
            }
        }
    }
}
