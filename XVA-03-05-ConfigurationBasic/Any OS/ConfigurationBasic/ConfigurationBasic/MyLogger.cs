using Serilog;
using XSockets.Logger;

namespace ConfigurationBasic
{
    /// <summary>
    /// To be able to see log info in the output window.
    /// </summary>
    public class MyLogger : XLogger
    {
        public MyLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Trace().MinimumLevel.Debug().CreateLogger();
        }
    }
}