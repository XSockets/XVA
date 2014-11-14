using System.Diagnostics;
using System.Threading;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace HitCounter
{
    [XSocketMetadata("hitcounter")]
    public class HitCounterController : XSocketController
    {
        // don't do it this way in real life!
        static int _count;
        public override void OnOpened()
        {
            Interlocked.Increment(ref _count);
            this.InvokeToAll(_count, "updateCount");
        }

        public override void OnClosed()
        {
            Interlocked.Decrement(ref _count);
            this.InvokeToAll(_count, "updateCount");
        }
    }
}