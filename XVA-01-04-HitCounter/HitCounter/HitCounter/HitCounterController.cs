using System.Threading;
using System.Threading.Tasks;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace HitCounter
{
    [XSocketMetadata("hitcounter")]
    public class HitCounterController : XSocketController
    {
        // don't do it this way in a real app!
        static int _count;

        public override async Task OnOpened()
        {
            Interlocked.Increment(ref _count);
            await this.InvokeToAll(_count, "updateCount");
        }

        public override async Task OnClosed()
        {
            Interlocked.Decrement(ref _count);
            await this.InvokeToAll(_count, "updateCount");
        }
    }
}