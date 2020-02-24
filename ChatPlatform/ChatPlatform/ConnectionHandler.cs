using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChatPlatform
{
    /// <summary>
    /// This class holds the information for incoming server connections.
    /// </summary>
    public class ConnectionHandler
    {
        /// <summary>
        /// A buffer for incoming data.
        /// </summary>
        private Byte[] bytes;
        /// <summary>
        /// A string buffer for converted incoming data.
        /// </summary>
        private String data;
        /// <summary>
        /// Holds the username of the connection.
        /// </summary>
        private string name;

        /// <summary>
        /// An event handler for events regarding incoming connections
        /// </summary>
        private EventHandler chatEventHandler;

        /// <summary>
        ///  Holds the TCP connection information
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// This object hooks into the TCP conenction to send bytes of information
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// This constructor creates a new object that holds the information of a client application.
        /// </summary>
        /// <param name="client">The incoming TcpClient pulled from the AcceptTcpClient method</param>
        /// <param name="name">The username of the incoming connection</param>
        /// <param name="chatEventHandler">The event handler for writing to the console</param>
        /// <param name="bufferSize">Desired buffer size for incoming data</param>
        public ConnectionHandler(TcpClient client, string name, EventHandler chatEventHandler, uint bufferSize)
        {
            bytes = new byte[bufferSize];
            this.chatEventHandler = chatEventHandler;
            this.name = name;

            this.client = client;
            stream = client.GetStream();
        }

        /// <summary>
        /// This loop waits for incoming data
        /// </summary>
        public void AwaitData()
        {
            try
            {
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    //Incoming data is stored in buffer
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    //Username, message_type, and message being parsed from the data
                    string name = data.Substring(0, data.IndexOf(':'));
                    MESSAGE_TYPE messageType = (MESSAGE_TYPE)Enum.Parse(typeof(MESSAGE_TYPE), data.Substring(data.IndexOf("//")+2));
                    string message = data.Substring(data.IndexOf(':')+1, data.IndexOf("//") - data.IndexOf(':') - 1);

                    //Calls an event to write to the console
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

        /// <summary>
        /// Sends a message back to the client
        /// </summary>
        /// <param name="message">The message being sent</param>
        public void SendMeMessage(Byte[] message)
        {
            stream.Write(message, 0, message.Length);
        }

        /// <summary>
        /// Public accessor for the username of the connection
        /// </summary>
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
