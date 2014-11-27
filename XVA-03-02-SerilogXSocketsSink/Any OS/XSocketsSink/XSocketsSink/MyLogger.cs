using Serilog;
using XSockets.Logger;

namespace XSocketsSink
{
    /// <summary>
    /// Setup logger to both Trace and XSockets-Sink
    /// </summary>
    public class MyLogger : XLogger
    {
        public MyLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.XSockets().WriteTo.Trace().MinimumLevel.Verbose().CreateLogger();
        }
    }
}