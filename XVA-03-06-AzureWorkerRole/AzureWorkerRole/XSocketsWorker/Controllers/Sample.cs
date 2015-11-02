using System.Diagnostics;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework;
using System.Threading.Tasks;

namespace XSocketsWorker.Controllers
{
    /// <summary>
    /// Not needed in the Azure hosting sample, just included as a basic sample.
    /// </summary>
    public class Sample : XSocketController
    {
        public override async Task OnMessage(IMessage message)
        {
            if(Debugger.IsAttached)
                Composable.GetExport<IXLogger>().Verbose("MyController:OnMessage {@m}",message);
            await this.InvokeToAll(message);
        }
    }
}
