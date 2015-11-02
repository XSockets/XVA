using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XSockets.Core.Common.Protocol;
using XSockets.Core.Common.Socket.Event.Arguments;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Protocol;

namespace CustomProtocolBasic
{ 

    /// <summary>
    /// This default protocol expects 0x00 as startbyte and 0xff as endbyte
    /// 
    /// To implement a custom communication protocol override the methods
    ///  - ReceiveData
    ///  - OnIncominFrame
    ///  - OnOutgoingFrame
    /// </summary>
    public class StupidProtocol : XSocketProtocol
    {
        public StupidProtocol()
        {
            this.ProtocolProxy = new StupidProtocolProxy();
        }
        /// <summary>
        /// Since the basic protocol will expect 0x00 and 0xFF (startbyte/endbyte)
        /// we implement (override) the receivedata method and just send the "frame" to be processed 
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="readState"></param>
        /// <param name="processFrame"></param>
        public override async Task ReceiveData(ArraySegment<byte> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                this.ReadState.Data.Add(data.Array[i]);                
            }
            await this.OnFrameReady(this.ReadState.Data, FrameType.Text);
            this.ReadState.Clear();
        }        
        /// <summary>
        /// Converts the incomming data in the form "controller|topic|data" into a IMessage     
        /// 
        /// Note that this stupid protocol wont be able to convert data in to complex objects   
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public override IMessage OnIncomingFrame(IEnumerable<byte> payload, MessageType messageType)
        {
            return this.ProtocolProxy.In(payload, messageType);
        }

        /// <summary>
        /// Converts a IMessage into a string of the form "controller|topic|data" to send back.
        /// Also add the trailing crlf        
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override byte[] OnOutgoingFrame(IMessage message)
        {
            return this.ProtocolProxy.Out(message);
        }
        
        /// <summary>
        /// The string to return after handshake
        /// </summary>
        public override string HostResponse
        {
            get { return "Welcome to StupidProtocol"; }
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
    }
}
