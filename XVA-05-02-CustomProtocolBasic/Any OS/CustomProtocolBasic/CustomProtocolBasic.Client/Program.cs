using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CustomProtocolBasic.Client
{
    /// <summary>
    /// Just a complement to the sample video where we use putty to communicate.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            const int bufferSize = 1024;
            //A subscription for the topic "bar" on the controller "foo", 0x12c means subscribe
            var subscription = Encoding.UTF8.GetBytes("foo|0x12c|bar");

            var client = new TcpClient("127.0.0.1", 4502);            
            var ns = client.GetStream();

            //Do stupid handshale
            var handshake = Encoding.UTF8.GetBytes("StupidProtocol");
            ns.Write(handshake, 0, handshake.Length);

            //Get handshake response
            var responseBuffer = new byte[bufferSize];
            ns.Read(responseBuffer, 0, bufferSize);

            //Extract response between 0x00 and 0xFF
            var response = Encoding.UTF8.GetString(responseBuffer.Skip(1).Take(responseBuffer.ToList().FindIndex(p => p == 0xff) - 1).ToArray()).Trim();

            //If ok, then start to listen for messages.
            if (response.Equals("Welcome to StupidProtocol", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine(response);
                //Subscribe for the topic "bar"
                ns.Write(subscription, 0, subscription.Length);

                var bytes = new byte[bufferSize];
                int i;
                while ((i = ns.Read(bytes, 0, bufferSize)) > 0)
                {
                    Console.Write(Encoding.UTF8.GetString(bytes.Take(i).ToArray()));
                    bytes = new byte[bufferSize];
                }
            }

            client.Close();
            ns.Close();
        }
    }
}
