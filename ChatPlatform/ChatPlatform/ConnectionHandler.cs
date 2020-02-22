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
        private string name;

        private EventHandler chatEventHandler;

        private TcpClient client;
        private NetworkStream stream;

        public ConnectionHandler(TcpClient client, string name, EventHandler chatEventHandler, uint bufferSize)
        {
            bytes = new byte[bufferSize];
            this.chatEventHandler = chatEventHandler;
            this.name = name;

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

                    string name = data.Substring(0, data.IndexOf(':'));
                    MESSAGE_TYPE messageType = (MESSAGE_TYPE)Enum.Parse(typeof(MESSAGE_TYPE), data.Substring(data.IndexOf("//")+2));
                    string message = data.Substring(data.IndexOf(':')+1, data.IndexOf("//") - data.IndexOf(':') - 1);

                    chatEventHandler?.Invoke(this, new MessageRecievedEventArgs(name, messageType, message));
                }
            }
            catch (System.IO.IOException)
            {
                ServerHandler.DisconnectClient(this);
            }
            catch (Exception)
            {
                Console.WriteLine("Error Occurred");
                ServerHandler.DisconnectClient(this);
            }
        }

        public void SendMeMessage(Byte[] message)
        {
            stream.Write(message, 0, message.Length);
        }

        public string Username
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}
