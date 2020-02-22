using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatPlatform
{
    class Server
    {
        
        public static List<ConnectionHandler> clientList = new List<ConnectionHandler>();
        public static event EventHandler ChatEventHandler;
        public static void Main(string[] args)
        {
            ChatEventHandler += RecieveMessage;

            ServerHandler.Start(13000, ChatEventHandler);
            ServerHandler.BeginAcceptConnections();
        }

        public static void RecieveMessage(object sender, EventArgs e)
        {
            Console.WriteLine((string)sender);
        }

        public static bool DisconnectClient(ConnectionHandler c)
        {
            return clientList.Remove(c);
        }
    }
}
