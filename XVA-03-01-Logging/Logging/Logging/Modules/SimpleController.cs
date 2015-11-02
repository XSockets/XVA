using System;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.XSocket;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace Logging.Modules
{
    [XSocketMetadata("Simple")]
    public class SimpleController : XSocketController
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
                Composable.GetExport<IXLogger>().Information("LogTest: {@model}", model); 
                //And... throw...
                throw new Exception("Ohh crap!");
            }
            catch (Exception ex)
            {
                Composable.GetExport<IXLogger>().Error("LogTest: {@ex}", ex);
            }
        }
    }
}
