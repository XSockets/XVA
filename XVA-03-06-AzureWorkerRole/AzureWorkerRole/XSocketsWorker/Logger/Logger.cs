using System;
using Serilog;
using XSockets.Logger;

namespace XSocketsWorker.Logger
{
    /// <summary>
    /// Not needed in the Azure hosting sample, but this will use Serilog to write to the Trace for better debugging.
    /// </summary>
    public class Logger : XLogger
    {
        public Logger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Trace().MinimumLevel.Verbose().CreateLogger();
        }
    }
}
