using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Plugin.Framework.Attributes;

namespace BinaryMessages
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time MVC controller
    /// </summary>
    [XSocketMetadata("fileshare")]
    public class FileShareController : XSocketController
    {

        public void FileShare(IMessage message)
        {
      
            this.InvokeToAll(message);

        }

    }
}
