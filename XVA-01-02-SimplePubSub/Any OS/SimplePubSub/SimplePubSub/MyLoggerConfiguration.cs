using System.IO;
using Serilog;
using XSockets.Logger;

namespace SimplePubSub
{
    // For details about configuration, visit http://serilog.net
    public class MyLoggerConfiguration : XLogger
    {
        public MyLoggerConfiguration()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .WriteTo.ColoredConsole()
            //.WriteTo.RollingFile(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Log\\Log-{Date}.txt"))
            .WriteTo.Trace()
            .CreateLogger();
        }
    }
}
