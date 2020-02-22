using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatPlatform
{
    class Program
    {
        public static event EventHandler ChatRecievedHandler;
        public static List<TcpClient> clientList = new List<TcpClient>();

        public static void Main(string[] args)
        {
            ChatRecievedHandler += RecieveMessage;

            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            //Commit Test

            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clientList.Add(client);

                Thread t = new Thread(()=> ClientHandler(client));
                t.Start();
            }
        }

        public static void RecieveMessage(object sender, EventArgs e)
        {
            Console.WriteLine((string)sender);
        }

        public static bool DisconnectClient(TcpClient c)
        {
            return clientList.Remove(c);
        }

        public static void ClientHandler(TcpClient client)
        {
            Byte[] bytes = new byte[256];
            String data = null;

            NetworkStream stream = client.GetStream();
            stream = client.GetStream();

            try
            {
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    ChatRecievedHandler?.Invoke(data, new EventArgs());
                }
            }
            catch (System.IO.IOException)
            {
                DisconnectClient(client);
            }
        }
    }
}
