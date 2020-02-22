using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    public class Client
    {
        static void Main(string[] args)
        {
            ClientHandler c = new ClientHandler(args[0], 13000, args[1]);

            try 
            { 
                while (true)
                {
                    Console.Write(">>");
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
