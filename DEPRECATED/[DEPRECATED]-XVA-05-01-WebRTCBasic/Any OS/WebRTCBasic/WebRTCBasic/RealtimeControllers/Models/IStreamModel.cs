using System;

namespace WebRTCBasic.RealtimeControllers.Models
{
    public interface IStreamModel
    {
        Guid Sender { get; set; }
        string StreamId { get; set; }
        string Description { get; set; }
    }
}