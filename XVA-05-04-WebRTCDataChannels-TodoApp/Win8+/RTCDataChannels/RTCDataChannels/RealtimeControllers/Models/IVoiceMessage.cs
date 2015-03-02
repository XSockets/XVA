using System;

namespace RTCDataChannels.RealtimeControllers.Models
{
    public interface IVoiceMessage
    {
        Guid Recipient { get; set; }
        Guid Sender { get; set; }
        Guid Id { get; set; }
        byte[] Bytes { get; set; }
        DateTime Created { get; set; }
    }
}