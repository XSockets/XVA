using Serilog;
using XSockets.Logger;

namespace MessagePipeline
{
    /// <summary>
    /// Override the default logger with a custom logger configuration.
    /// 
    /// To get more information about how to configure Serilog see http://serilog.net/
    /// </summary>
    public class MyLogger : XLogger
    {
        public MyLogger()
        {            
            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Trace().CreateLogger();
        }
    }
}