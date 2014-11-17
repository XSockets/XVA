using System;
using System.Timers;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace ClockController
{
    /// <summary>
    /// There will only be one instance of this class
    /// A singleton...
    /// </summary>
    [XSocketMetadata("ServerTimer", PluginRange.Internal)]
    public class ServerTimerController : XSocketController
    {        
        public ServerTimerController()
        {
            var t = new Timer(1000);
            t.Elapsed += (s, e) => this.InvokeToAll<ClockController>(DateTime.Now.ToString("hh:mm:ss"), "tick");
            t.Start();
        }
    }
}
