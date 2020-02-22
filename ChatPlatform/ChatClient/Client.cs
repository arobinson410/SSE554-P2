using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    public enum MESSAGE_TYPE
    {
        LOGIN,
        DISCONNECT,
        MESSAGE_SENT,
    };

    public class Client
    {
        private static TcpClient client;
        private static NetworkStream stream;

        private static string username;

        static void Main(string[] args)
        {
            ClientHandler c = new ClientHandler(args[0], 13000, args[1]);

            try 
            { 
                while (true)
                {
                    string incomingMessage = Console.ReadLine();
                    c.SendMessage(incomingMessage);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Could not Connect!");
            }
            finally
            {
                c.StopClient();
            }
        }
    }
}
