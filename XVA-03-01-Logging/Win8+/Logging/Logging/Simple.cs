using System;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.XSocket;
using XSockets.Plugin.Framework;

namespace Logging
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time controller
    /// </summary>
    public class Simple : XSocketController
    {
        /// <summary>
        /// Log the incomming data and send it back to the caller only
        /// </summary>
        /// <param name="model"></param>
        public void LogTest(SomeModel model)
        {
            try
            {
                //Get the logger and write with level information
                Composable.GetExport<IXLogger>().Information("LogTest: {now}, {@model}", DateTime.Now, model);
                //And... throw...
                throw new Exception("Ohh crap!");
            }
            catch (Exception ex)
            {
                Composable.GetExport<IXLogger>().Error("LogTest: {now}, {message}", DateTime.Now, ex.Message);
            }
        }
    }
}