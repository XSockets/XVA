using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSockets.Core.Common.Globals;
using XSockets.Core.Common.Protocol;
using XSockets.Core.Common.Socket.Event.Arguments;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.Common.Utility.Serialization;
using XSockets.Core.XSocket.Model;
using XSockets.Plugin.Framework;

namespace CustomProtocolBasic
{
    public class StupidProtocolProxy : IProtocolProxy
    {
        private IXSocketJsonSerializer JsonSerializer { get; set; }

        public StupidProtocolProxy()
        {
            JsonSerializer = Composable.GetExport<IXSocketJsonSerializer>();
        }
        public IMessage In(IEnumerable<byte> payload, MessageType messageType)
        {
            var data = Encoding.UTF8.GetString(payload.ToArray());
            if (data.Length == 0) return null;
            var d = data.Split('|');
            switch (d[1])
            {
                case Constants.Events.PubSub.Subscribe:
                    return new Message(new XSubscription { Topic = d[2] }, Constants.Events.PubSub.Subscribe, d[0], JsonSerializer);             
                case Constants.Events.PubSub.Unsubscribe:
                    return new Message(new XSubscription { Topic = d[2] }, Constants.Events.PubSub.Unsubscribe, d[0], JsonSerializer);

                case Constants.Events.Storage.Set:
                    var kv = d[2].Split(',');
                    return new Message(new XStorage { Key = kv[0], Value = kv[1] }, Constants.Events.Storage.Set, d[0], JsonSerializer);
                case Constants.Events.Storage.Get:
                    return new Message(new XStorage { Key = d[2] }, Constants.Events.Storage.Get, d[0], JsonSerializer);
                case Constants.Events.Storage.Remove:
                    return new Message(new XStorage { Key = d[2] }, Constants.Events.Storage.Remove, d[0], JsonSerializer);
                default:
                    return new Message(d[2], d[1], d[0], JsonSerializer);

            }
        }

        public byte[] Out(IMessage message)
        {
            if (message.Topic == Constants.Events.Controller.Opened)
            {
                var c = this.JsonSerializer.DeserializeFromString<ClientInfo>(message.Data);
                var d = string.Format("PI:{0},CI:{1}", c.PersistentId, c.ConnectionId);
                message = new Message(d, message.Topic, message.Controller);

            }
            return Encoding.UTF8.GetBytes(string.Format("{0}|{1}|{2}\r\n", message.Controller, message.Topic, message.Data));
        }
    }
}
