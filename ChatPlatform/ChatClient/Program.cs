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
            String server = "127.0.0.1";

            Thread.Sleep(5000);

            TcpClient client = new TcpClient(server, port);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes("Client Sending Message");

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);

            stream.Close();
            client.Close();
        }
    }
}
