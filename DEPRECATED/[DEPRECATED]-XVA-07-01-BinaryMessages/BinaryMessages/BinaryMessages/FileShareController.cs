using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;
using System.Threading.Tasks;

namespace BinaryMessages
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time controller
    /// </summary>
    [XSocketMetadata("fileshare")]
    public class FileShareController : XSocketController
    {
        public async Task FileShare(IMessage message)
        {      
            await this.InvokeToAll(message);
        }

        public async Task FIleShareChunked(IMessage message)
        {
            await this.InvokeToAll(message);
        }
    }
}
