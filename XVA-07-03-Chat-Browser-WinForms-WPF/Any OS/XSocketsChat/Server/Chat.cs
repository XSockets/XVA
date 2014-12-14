using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.Common.Socket.Event.Interface;

namespace Server
{
    public enum TargetLocation
    {
        All,
        Sweden,
        Norway,
        Denmark
    }
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time MVC controller
    /// </summary>
    public class Chat : XSocketController
    {
        public TargetLocation Location { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// If target is set to all we broadcast to all, otherwise only send to client with
        /// the same location as the sender.
        /// 
        /// Very powerful with dynamic targeting using lambda expressions. 
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if(this.Location == TargetLocation.All)
                this.InvokeToAll(string.Format("{0}: {1}/{2}",UserName,Location,message),"addMessage");
            else            
                this.InvokeTo(p => p.Location == Location, string.Format("{0}: {1}/{2}", UserName, Location, message), "addMessage");            
        }
    }
}
