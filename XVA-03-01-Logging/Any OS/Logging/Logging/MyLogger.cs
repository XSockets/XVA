using Serilog;
using XSockets.Logger;

namespace Logging
{
    /// <summary>
    /// Custom plugin that will override the default logger.
    /// This logger will write to the Trace and have a minimum level = "Information"
    /// 
    /// For more info about configuration se Serilog.NET
    /// </summary>
    public class MyLogger : XLogger
    {
        public MyLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Trace().MinimumLevel.Information().CreateLogger();
        }
    }
}