using System;
using System.Threading;
using Microsoft.SPOT;
using XSockets.ClientMF42;
using XSockets.ClientMF42.Event.Arguments.Interfaces;
using XSockets.ClientMF42.Interfaces;

namespace NetMF.Client
{
    public class Program
    {
        private static IXSocketClient _client;
        private static Thread sensorThread;
        private static int threshold = 25;
        public static void Main()
        {
            _client = new XSocketClient("127.0.0.1", 4502, "JsonProtocol", "Welcome to JsonProtocol");

            _client.OnOpen += (sender, args) =>
            {
                Debug.Print("Connected");
                // Tell the server that I am a Netduino
                _client.SetEnum("Hardware","Netduino","Sensor");
                
                //Start sending fake sensor data
                sensorThread = new Thread(Sensor);
                sensorThread.Start();
            };
            _client.OnClose += (sender, args) =>
            {
                Debug.Print("Closed");
                sensorThread.Suspend();
            };
            //Listen for messages
            _client.OnMessage += _client_OnMessage;

            //Open connection
            _client.Open();            

            //Prevent exit since that would stop the program
            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// When a message arrives, check if the topic was "threshold" 
        /// and if so set the new threshold value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _client_OnMessage(object sender, IMessage e)
        {
            Debug.Print("OnMessage Topic: " + e.T);
            Debug.Print("OnMessage Data: " + e.D);
                        
            switch (e.T)
            {
                case "threshold":
                    threshold = int.Parse(e.D);
                    Debug.Print("New threshold on NETMF device: " + threshold);
                    break;
            }
        }

        /// <summary>
        /// Just tick data every second 
        /// </summary>
        public static void Sensor()
        {
            while (true)
            {                
                //Send data if lower than or equal to the threshold set from the client
                var r = new Random().Next(50);                
                if(r <= threshold)
                    _client.Publish("change", r, "sensor");

                Thread.Sleep(1000);
            }
        }
    }
}
