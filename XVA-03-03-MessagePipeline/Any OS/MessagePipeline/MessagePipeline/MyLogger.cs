using Serilog;
using XSockets.Logger;

namespace MessagePipeline
{
    public class MyLogger : XLogger
    {
        public MyLogger()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Trace().CreateLogger();
        }
    }
}