using XSockets.Core.XSocket;
using XSockets.Plugin.Framework.Attributes;

namespace ClockController
{
    /// <summary>
    /// Emtpy controller that client connect to
    /// The tick will come from the singleton internal controller
    /// This is possible since XSockets allows communication cross controllers 
    /// (see ServerTimerController)
    /// </summary>
    [XSocketMetadata("clock")]
    public class ClockController : XSocketController{}
}