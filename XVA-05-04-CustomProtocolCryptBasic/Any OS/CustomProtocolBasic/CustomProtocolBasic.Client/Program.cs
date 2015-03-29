using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using CryptHelpers;

namespace CustomProtocolBasic.Client
{
    /// <summary>
    /// Just a complement to the sample video where we use putty to communicate.
    /// </summary>
    class Program
    {
        //These two values should not be hard coded in your code.
        private static byte[] key = {251, 9, 67, 117, 237, 158, 138, 150, 255, 97, 103, 128, 183, 65, 76, 161, 7, 79, 244, 225, 146, 180, 51, 123, 118, 167, 45, 10, 184, 181, 202, 190};
        private static byte[] vector = {214, 11, 221, 108, 210, 71, 14, 15, 151, 57, 241, 174, 177, 142, 115, 137};

        
        static void Main(string[] args)
        {
            const int bufferSize = 1024;
            //A subscription for the topic "bar" on the controller "foo", 0x12c means subscribe
            var subscription = "foo|0x12c|bar";

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
                var encrypted = Encrypt(subscription);
                ns.Write(encrypted, 0, encrypted.Length);

                var bytes = new byte[bufferSize];
                int i;
                while ((i = ns.Read(bytes, 0, bufferSize)) > 0)
                {
                    Console.Write(Decrypt(bytes.Take(i).ToArray()));
                    bytes = new byte[bufferSize];
                }
            }

            client.Close();
            ns.Close();
        }

        private static byte[] Encrypt(string s)
        {
            using (var rijndaelHelper = new RijndaelHelper(key, vector))
            {
                return rijndaelHelper.Encrypt(s);                
            }
        }
        private static byte[] Decrypt(byte[] b)
        {
            using (var rijndaelHelper = new RijndaelHelper(key, vector))
            {
                return rijndaelHelper.Decrypt(b);
            }
        }
    }
}
