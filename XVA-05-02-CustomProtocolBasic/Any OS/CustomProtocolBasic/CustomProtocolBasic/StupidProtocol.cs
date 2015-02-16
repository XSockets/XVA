using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XSockets.Core.Common.Protocol;
using XSockets.Core.Common.Socket.Event.Arguments;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket.Model;
using XSockets.Protocol;

namespace CustomProtocolBasic
{
    /// <summary>
    /// This default protocol expects 0x00 as startbyte and 0xff as endbyte
    /// 
    /// To implement a custom communication protocol override the methods
    ///  - ReceiveData
    ///  - OnIncominFrame and 
    ///  - OnOutgoingFrame
    /// </summary>
    public class StupidProtocol : XSocketProtocol
    {
        /// <summary>
        /// Since the basic protocol will expect 0x00 and 0xFF (startbyte/endbyte)
        /// we implement (override) the receivedata method and just send the "frame" to be processed 
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="readState"></param>
        /// <param name="processFrame"></param>
        public override void ReceiveData(ArraySegment<byte> segment, IReadState readState, Action<FrameType, IList<byte>> processFrame)
        {      
            readState.Data.AddRange(segment.Array.Take(segment.Count));
            processFrame(FrameType.Text, readState.Data);
            readState.Data.Clear();
            readState.Clear();    
        }

        /// <summary>
        /// Converts the incomming data in the form "controller|topic|data" into a IMessage     
        /// 
        /// Note that this stupid protocol wont be able to convert data in to complex objects   
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public override IMessage OnIncomingFrame(IList<byte> payload, MessageType messageType)
        {
            //Remove any crlf that migth be added by for example Putty or other clients.
            var data = Encoding.UTF8.GetString(payload.ToArray()).TrimEnd('\r', '\n');
            //If there was nothing but the crlf, just return null
            if (data.Length == 0) return null;
            //Split on the delimiter to be able to know controller, topic and data
            var d = data.Split('|');
            //Convert into IMessage
            switch (d[1])
            {
                //If the topic is a publish or subscribe
                case XSockets.Core.Common.Globals.Constants.Events.PubSub.Subscribe: // 0x12c
                case XSockets.Core.Common.Globals.Constants.Events.PubSub.Unsubscribe: // 0x12d
                    return new Message(new XSubscription { Topic = d[2] }, d[1], d[0], this.JsonSerializer);

                //Plain message....
                default:
                    return new Message(d[2], d[1], d[0], this.JsonSerializer);
            }
        }

        /// <summary>
        /// Converts a IMessage into a string of the form "controller|topic|data" to send back.
        /// Also add the trailing crlf        
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override byte[] OnOutgoingFrame(IMessage message)
        {
            return
                Encoding.UTF8.GetBytes(string.Format("{0}|{1}|{2}\r\n", message.Controller, message.Topic, message.Data));
        }

        #region Methods and properties implemented by the Protocol Template
        /// <summary>
        /// Extract the path (controller name) from the handshake
        /// </summary>
        public Regex GetPathRegex
        {
            get { return new Regex(@".+?(?= " + this.ProtocolPattern + ")", RegexOptions.None); }
        }

        /// <summary>
        /// A simple identifier fot the protocol since the ProtocolPattern might be complex or unfriendly to read.
        /// </summary>
        public override string ProtocolIdentifier
        {
            get { return "StupidProtocolIdentifier"; }
        }

        /// <summary>
        /// The string to identify the protocol in the handshake
        /// </summary>
        public override string ProtocolPattern
        {
            get { return "StupidProtocol"; }
        }

        /// <summary>
        /// The string to return after handshake
        /// </summary>
        public override string HostResponse
        {
            get { return "Welcome to StupidProtocol"; }
        }

        /// <summary>
        /// Perform any extra logic for handshake, build a hostresponse etc
        /// </summary>
        /// <returns></returns>
        public override bool DoHandshake()
        {
            Response = HostResponse;
            return true;
        }

        /// <summary>
        /// Set to true if your clients connected to this protocol will return pong on ping.
        /// </summary>
        /// <returns></returns>
        public override bool CanDoHeartbeat()
        {
            return false;
        }        

        public override IXSocketProtocol NewInstance()
        {
            return new StupidProtocol();
        }
        #endregion
    }
}
