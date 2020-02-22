using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatPlatform
{
    public static class ServerHandler
    {
        private static bool isReady = false;
        private static bool acceptRunning = false;

        private static List<ConnectionHandler> clientList = new List<ConnectionHandler>();

        private static TcpListener server;
        private static event EventHandler chatEventHandler;

        public static bool Start(int port, EventHandler eventHandler)
        {
            try
            {
                chatEventHandler = eventHandler;

                server = new TcpListener(IPAddress.Any, port);
                server.Start();

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
                Thread t = new Thread(new ThreadStart(BeginAcceptConnection));
                t.Start();
            }
        }

        private static void BeginAcceptConnection()
        {
            try
            {
                acceptRunning = true;

                while (true)
                {
                    ConnectionHandler c = new ConnectionHandler(server.AcceptTcpClient(), chatEventHandler, 256);
                    clientList.Add(c);

                    Thread t = new Thread(() => c.AwaitData());
                    t.Start();
                }
            }
            catch(Exception)
            {
                acceptRunning = false;
            }
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
