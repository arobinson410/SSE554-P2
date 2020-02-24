using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChatPlatform
{
    /// <summary>
    /// This class holds all the information for the server. It's contained in its own class so that multiple instances can be spawned if need be.
    /// </summary>
    public static class ServerHandler
    {
        /// <summary>
        /// Handles all incoming messages
        /// </summary>
        public static event EventHandler ChatEventHandler;
        /// <summary>
        /// Holds a list of connected clients
        /// </summary>
        public static List<ConnectionHandler> ClientList = new List<ConnectionHandler>();

        private static bool isReady = false;
        private static bool acceptRunning = false;

        private static TcpListener server;

        /// <summary>
        /// Instantiates all required objects to run a server
        /// </summary>
        /// <param name="port">Startup port</param>
        /// <returns>Returns true if the server was sucessfully setup</returns>
        public static bool Start(int port)
        {
            // Subscribe the Event handler to the receive message function.
            ChatEventHandler += RecieveMessage;

            // Attempt to start a new TCP Listener server.
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();

                // Set ready flag to true.
                isReady = true;
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Allows the server to start accepting connetions
        /// </summary>
        public static void BeginAcceptConnections()
        {
            if (!acceptRunning)
            {
                Thread t = new Thread(new ThreadStart(PrivateBeginAcceptConnections));
                t.Start();
            }
        }

        /// <summary>
        /// Private method that runs when a new connection thread is created.
        /// </summary>
        private static void PrivateBeginAcceptConnections()
        {
            try
            {
                acceptRunning = true;

                while (true)
                {
                    Debug.WriteLine("Creating new Connection...");
                    ConnectionHandler c = new ConnectionHandler(server.AcceptTcpClient(), "1", ChatEventHandler, 256);
                    ClientList.Add(c);
                    Debug.WriteLine("Connection Created.");
                    Thread t = new Thread(() => c.AwaitData());
                    t.Start();

                }
            }
            catch(Exception)
            {
                acceptRunning = false; 
            }
        }

        /// <summary>
        /// Method called when the ChatEventHandler event is invoked
        /// </summary>
        /// <param name="sender">The ConnectionHandler responsible for invoking the method</param>
        /// <param name="e">The arguments containing the username, message type, and message</param>
        public static void RecieveMessage(object sender, EventArgs e)
        {
            MessageRecievedEventArgs m = e as MessageRecievedEventArgs;
            ConnectionHandler c = sender as ConnectionHandler;

            switch (m.t)
            {
                case MESSAGE_TYPE.LOGIN:
                    c.Username = m.sender;
                    Console.WriteLine(c.Username + " connected.");
                    Broadcast(c, c.Username + " connected.");
                    break;
                case MESSAGE_TYPE.MESSAGE_SENT:
                    Console.WriteLine(m.sender + ": " + m.message);
                    Broadcast(c, c.Username + ": " + m.message);
                    break;
                case MESSAGE_TYPE.DISCONNECT:
                    Console.WriteLine(m.sender + " disconnected.");
                    Broadcast(c, c.Username + " disconnected.");
                    break;
                default:
                    break;
            }

            Console.WriteLine(ClientList.Count);
        }

        /// <summary>
        /// This method sends a message back to all clients except the client that sent the message
        /// </summary>
        /// <param name="sender">The client that sent the message</param>
        /// <param name="message">The message being sent</param>
        public static void Broadcast(ConnectionHandler sender, string message)
        {
            foreach(ConnectionHandler c in ClientList)
            {
                if (!c.Equals(sender))
                {
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    c.SendMeMessage(data);
                }
            }
        }

        /// <summary>
        /// This methods ensure that the disconnecting client is removed from the list.
        /// </summary>
        /// <param name="h">Client that is disconnecting</param>
        public static void DisconnectClient(ConnectionHandler h)
        {
            ChatEventHandler?.Invoke(h, new MessageRecievedEventArgs(h.Username, MESSAGE_TYPE.DISCONNECT, ""));
            ClientList.Remove(h);
        }

        /// <summary>
        /// Closes the server
        /// </summary>
        public static void Stop()
        {
            server.Stop();
        }

        /// <summary>
        /// See if the server is ready to start
        /// </summary>
        /// <returns></returns>
        public static bool IsReady()
        {
            return isReady;
        }     
    }
}
