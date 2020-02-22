using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    public class Client
    {
        private static TcpClient client;
        private static NetworkStream stream;

        static void Main(string[] args)
        {
            SetupClient(args[0], 13000);

            try 
            { 
                while (true)
                {
                    string incomingMessage = Console.ReadLine();
                    SendMessage(incomingMessage);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Could not Connect!");
            }
            finally
            {
                StopClient();
            }
        }

        public static void SendMessage(string s)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
            stream.Write(data, 0, data.Length);
        }

        public static void SetupClient(String address, Int32 port)
        {
            client = new TcpClient(address, port);
            stream = client.GetStream();
        }

        public static void StopClient()
        {
            stream.Close();
            client.Close();
        }
    }
}
