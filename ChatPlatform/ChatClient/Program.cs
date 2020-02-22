using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Int32 port = 13000;
            String server = args[0];

            try
            {
                TcpClient client = new TcpClient(server, port);

                while (true)
                {
                    string incomingMessage = Console.ReadLine();
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(incomingMessage);

                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                }
            }
            catch(Exception)
            {
                Console.WriteLine("Could not Connect!");
            }

            //stream.Close();
            
        }
    }
}
