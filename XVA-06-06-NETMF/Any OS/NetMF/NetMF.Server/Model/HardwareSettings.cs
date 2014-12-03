namespace NetMF.Server.Model
{
    /// <summary>
    /// To set a threshold on a specific hardware
    /// </summary>
    public class HardwareSettings
    {
        public Hardware Hardware { get; set; }
        public int Threshold { get; set; }
    }
}