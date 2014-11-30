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
        public override void OnIncomingMessage(IXSocketController controller, IMessage e)
        {
            Composable.GetExport<IXLogger>().Debug("Incoming data: {@m}",e);
            base.OnIncomingMessage(controller, e);
        }

        public override IMessage OnOutgoingMessage(IXSocketController controller, IMessage e)
        {
            Composable.GetExport<IXLogger>().Debug("Outgoing data: {@m}", e);
            return base.OnOutgoingMessage(controller, e);
        }
    }
}
