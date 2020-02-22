using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChatPlatform
{
    public class ConnectionHandler
    {
        private Byte[] bytes;
        private String data;

        private EventHandler chatEventHandler;

        private TcpClient client;
        private NetworkStream stream;

        public ConnectionHandler(TcpClient client, EventHandler chatEventHandler, uint bufferSize)
        {
            bytes = new byte[bufferSize];
            this.chatEventHandler = chatEventHandler;

            this.client = client;
            stream = client.GetStream();
        }

        public void AwaitData()
        {
            try
            {
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    chatEventHandler?.Invoke(data, new EventArgs());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred");
            }
        }
    }
}
