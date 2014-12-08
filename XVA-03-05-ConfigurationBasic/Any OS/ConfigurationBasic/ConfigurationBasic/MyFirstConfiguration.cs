using System;
using XSockets.Core.Configuration;

namespace ConfigurationBasic
{
    /// <summary>
    /// My first config, setup a endpoint for XSockets.
    /// When we create custom configs the server will not create auto configurations.
    /// </summary>
    public class MyFirstConfiguration : ConfigurationSetting
    {
        public MyFirstConfiguration()
            : base(new Uri("ws://127.0.0.1:81"))
        {

        }
    }
}
