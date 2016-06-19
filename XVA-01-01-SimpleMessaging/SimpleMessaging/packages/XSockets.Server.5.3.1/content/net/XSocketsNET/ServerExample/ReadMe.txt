Showing howto start a XSockets.NET server, see more under XSockets.NET 5 templates

//Console Application
using XSockets.Core.Common.Socket;
using XSockets.Plugin.Framework;
using (var container = Composable.GetExport<IXSocketServerContainer>())
{
    container.Start();
    Console.ReadLine();
}