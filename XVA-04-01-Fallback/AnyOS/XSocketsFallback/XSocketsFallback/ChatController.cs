using System;
using System.Timers;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace XSocketsFallback
{
    [XSocketMetadata("Tick", PluginRange.Internal)]
    public class TickController : XSocketController
    {
        public TickController()
        {
            var t = new Timer(23000);
            t.Elapsed +=
                (sender, args) => this.InvokeToAll<ChatController>(string.Format("Tick {0}", DateTime.Now), "tick");
            t.Start();
        }
    }


    [XSocketMetadata("chat")]
    public class ChatController : XSocketController
    {
        /// <summary>
        /// This will publish any message to the client being subscribers of the "Topic" part of the IMessage
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(IMessage message)
        {            
            this.PublishToAll(message);
        }        
    }
}
