using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatPlatform
{
    /// <summary>
    /// Main server class containing the main method, which is run on server execution
    /// </summary>
    class Server
    { 
        public static void Main(string[] args)
        {
            ServerHandler.Start(13000);
            ServerHandler.BeginAcceptConnections();
        } 
    }
}
