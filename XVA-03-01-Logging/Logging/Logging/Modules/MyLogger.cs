namespace Logging.Modules
{
    /// <summary>
    /// Custom plugin that will override the default logger.
    /// This logger will write to the Trace and have a minimum level = "Information"
    /// 
    /// For more info about configuration se Serilog.NET
    /// </summary>
    using System.IO;
    using Serilog;
    using XSockets.Logger;

    public class DefaultLogger : XLogger
    {
        public DefaultLogger()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.ColoredConsole()
                //.WriteTo.RollingFile(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Log\\Log-{Date}.txt"))
                .WriteTo.Trace()
                .CreateLogger();
        }
    }
}