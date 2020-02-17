using System;
using System.Net;
using System.Net.Sockets;

namespace ChatPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            TcpListener server = new TcpListener(localAddr, port);

            server.Start();
            Byte[] bytes = new byte[256];
            String data = null;

            TcpClient client = server.AcceptTcpClient();

            NetworkStream stream = client.GetStream();

            int i;
            while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Recieved: {0}", data);
            }
        }
    }
}
