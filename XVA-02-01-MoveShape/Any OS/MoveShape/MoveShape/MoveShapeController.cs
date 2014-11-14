using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace MoveShape
{
    [XSocketMetadata("moveshape")]
    public class MoveShapeController : XSocketController
    {
        public void Move(int x, int y)
        {
            this.InvokeToOthers(new { x, y }, "move");
        }
    }
}
