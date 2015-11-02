using System.Threading.Tasks;
using XSockets.Core.Common.Socket;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.XSocket;
using XSockets.Plugin.Framework;

namespace MessagePipeline
{
    /// <summary>
    /// Override the default pipeline to interrupt messages going in/out
    /// 
    /// We only write the messages (in/out) to the custom logger
    /// </summary>
    public class MyPipeline : XSocketPipeline
    {
        public override async Task OnIncomingMessage(IXSocketController controller, IMessage e)
        {
            Composable.GetExport<IXLogger>().Information("Incoming data: {@m}",e);
            await base.OnIncomingMessage(controller, e);
        }

        public override async Task<IMessage> OnOutgoingMessage(IXSocketController controller, IMessage e)
        {
            Composable.GetExport<IXLogger>().Information("Outgoing data: {@m}", e);
            return await base.OnOutgoingMessage(controller, e);
        }
    }
}
