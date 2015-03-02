using System;

namespace RTCDataChannels.RealtimeControllers.Models
{
    public interface IPeerConnection
    {
        Guid Context { get; set; }
        Guid PeerId { get; set; }
    }
}