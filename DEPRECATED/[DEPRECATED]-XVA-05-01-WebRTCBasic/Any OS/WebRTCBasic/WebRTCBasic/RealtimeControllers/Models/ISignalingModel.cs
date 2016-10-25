using System;

namespace WebRTCBasic.RealtimeControllers.Models
{
    public interface ISignalingModel
    {
        string Message { get; set; }
        Guid Recipient { get; set; }
        Guid Sender { get; set; }
    }
}