using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

///
namespace ChatClient
{
    /// <summary>
    /// This class manages the connection to the server. It is contained in its own object so that multiple can be dynamically spawned if more than one client connection is needed.
    /// </summary>
    public class ClientHandler
    {
        /// <summary>
        ///  Holds the TCP connection information
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// This object hooks into the TCP conenction to send bytes of information
        /// </summary>
        private NetworkStream stream;
        /// <summary>
        /// This variable holds the username that is passed through the constructor
        /// </summary>
        private string username;

        /// <summary>
        /// This event handler keeps track of incoming messages and allows methods, like the PrintMessage() method to hook into the event called when a message is recieved.
        /// </summary>
        public event EventHandler ChatRecievedEventHandler;

        /// <summary>
        /// This constructor creates an instance of the ClientHandler.
        /// </summary>
        /// <param name="address">The IP Address of the server</param>
        /// <param name="port">The port the server is running on</param>
        /// <param name="username">The username of the client</param>
        public ClientHandler(string address, Int32 port, string username)
        {
            client = new TcpClient(address, port);
            stream = client.GetStream();
            this.username = username;

            //Subscribes incoming events to the PrintMessage delegate.
            ChatRecievedEventHandler += PrintMessage;

            //Sends the login message to the server so it knows what the client username is
            SendLoginMessage();
            //Spawn a thread to wait for messages
            Thread t = new Thread(new ThreadStart(ReceiveMessageFromServer));
            t.Start();
        }

        /// <summary>
        /// This destructor makes sure that the client is stopped when the object is destructed.
        /// </summary>
        ~ClientHandler()
        {
            StopClient();
        }

        /// <summary>
        /// Sends the login message to the server so it knows what the client username is.
        /// </summary>
        public void SendLoginMessage()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(username + ":" + "" + "//" + MESSAGE_TYPE.LOGIN);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// This loop runs on a separate thread so that the client can recieve messages that other clients have sent.
        /// </summary>
        public void ReceiveMessageFromServer()
        {
            while(true)
            {
                Byte[] data = new byte[256];
                if (stream.Read(data, 0, data.Length) != 0)
                {
                    ChatRecievedEventHandler?.Invoke(this, new MessageRecievedEventArgs(Encoding.ASCII.GetString(data).Replace("\0", "")));
                }
            }
        }

        /// <summary>
        /// This message allows the ChatRecievedEventHandler to print to the output window 
        /// </summary>
        /// <param name="sender">ClientHander responsible to invoking the ChatRecievedEventHandler event</param>
        /// <param name="e">The MessageRecievedEventArgs sent by invoking the event</param>
        public void PrintMessage(object sender, EventArgs e)
        {
            MessageRecievedEventArgs m = e as MessageRecievedEventArgs;
            Console.WriteLine(m.message);
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="s">The message being sent</param>
        public void SendMessage(string s)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(username + ":" + s + "//" + MESSAGE_TYPE.MESSAGE_SENT);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// This method closes the TCP and Network streams.
        /// </summary>
        public void StopClient()
        {
            stream.Close();
            client.Close();
        }

    }
}
