using XSockets.Core.Configuration;

namespace Server
{
    public class MyConfig : ConfigurationSetting
    {
        public MyConfig() : base("ws://localhost:8080")
        {
            
        }
    }
}