using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.Common.Configuration;
using XSockets.Core.Common.Socket;
using XSockets.Core.Configuration;

[assembly: PreApplicationStartMethod(typeof(ConfigurationBasic.Startup), "Start")]
namespace ConfigurationBasic
{
    public static class Startup
    {
        private static IXSocketServerContainer container;
        public static void Start()
        {
            container = XSockets.Plugin.Framework.Composable.GetExport<IXSocketServerContainer>();
            container.Start();

            //// You can also add custom confgurations at startup.
            //var configs = new List<IConfigurationSetting>();
            //for (var i = 83; i <= 85; i++)
            //{
            //    configs.Add(new ConfigurationSetting("ws://localhost:" + i));
            //}
            //container.Start(configurationSettings: configs);
        }
    }
}
