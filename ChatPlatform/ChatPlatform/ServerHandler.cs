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
    

    public static class ServerHandler
    {
        public static event EventHandler ChatEventHandler;
        public static List<ConnectionHandler> ClientList = new List<ConnectionHandler>();

        private static bool isReady = false;
        private static bool acceptRunning = false;

        private static TcpListener server;

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

        public static void BeginAcceptConnections()
        {
            if (!acceptRunning)
            {
                Thread t = new Thread(new ThreadStart(PrivateBeginAcceptConnections));
                t.Start();
            }
        }

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

        public static void RecieveMessage(object sender, EventArgs e)
        {
            MessageRecievedEventArgs m = e as MessageRecievedEventArgs;
            ConnectionHandler c = sender as ConnectionHandler;

            switch (m.t)
            {
                case MESSAGE_TYPE.LOGIN:
                    c.Username = m.sender;
                    Console.WriteLine(c.Username + " connected.");
                    break;

                case MESSAGE_TYPE.MESSAGE_SENT:
                    Console.WriteLine(m.sender + ": " + m.message);
                    Broadcast(c, m.message);
                    break;
                case MESSAGE_TYPE.DISCONNECT:
                    Console.WriteLine(m.sender + " disconnected.");
                    break;
                default:
                    break;
            }
            Console.WriteLine(ClientList.Count);
        }

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

        public static void DisconnectClient(ConnectionHandler h)
        {
            ChatEventHandler?.Invoke(h, new MessageRecievedEventArgs(h.Username, MESSAGE_TYPE.DISCONNECT, ""));
            ClientList.Remove(h);
        }

        public static void Stop()
        {
            server.Stop();
        }

        public static bool IsReady()
        {
            return isReady;
        }

        
    }
}
