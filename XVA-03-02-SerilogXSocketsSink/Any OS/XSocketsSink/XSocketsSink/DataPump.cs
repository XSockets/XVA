using System;
using System.Timers;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.XSocket;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace XSocketsSink
{
    /// <summary>
    /// Will pump messages to the log event 5 sec with random levels    
    /// </summary>
    [XSocketMetadata("DataPump", PluginRange.Internal)]
    public class DataPump : XSocketController
    {
        private const string LogTemplate = "This is a Serilog.Sinks.XSockets test with level {0}";
        public DataPump()
        {
            var t = new Timer(3000);
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            var level = (LogEventLevel)new Random().Next(0, 6);
            switch (level)
            {
                case LogEventLevel.Verbose:
                    Composable.GetExport<IXLogger>().Verbose(LogTemplate, level);
                    break;
                case LogEventLevel.Debug:
                    Composable.GetExport<IXLogger>().Debug(LogTemplate, level);
                    break;
                case LogEventLevel.Information:
                    Composable.GetExport<IXLogger>().Information(LogTemplate, level);
                    break;
                case LogEventLevel.Warning:
                    Composable.GetExport<IXLogger>().Warning(LogTemplate, level);
                    break;
                case LogEventLevel.Error:
                    Composable.GetExport<IXLogger>().Error(LogTemplate, level);
                    break;
                case LogEventLevel.Fatal:
                    Composable.GetExport<IXLogger>().Fatal(LogTemplate, level);
                    break;                
            }

        }
    }
}