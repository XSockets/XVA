using System.Threading.Tasks;
//using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace MoveShape
{
    [XSocketMetadata("moveshape")]
    public class MoveShapeController : XSocketController
    {
        public async Task Move(int x, int y)
        {
            await this.InvokeToOthers(new { x, y }, "move");
        }

        ///// <summary>
        ///// This will be more efficient but do the same thing.
        ///// Since we just use IMessage we do not serialize/deserialize for no reason like the sample above does.
        ///// </summary>
        ///// <param name="message"></param>
        //public async Task Move(IMessage message)
        //{
        //    await this.InvokeToOthers(message);
        //}
    }
}
