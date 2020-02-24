using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// This namespace holds all the references to the client application.
/// </summary>
namespace ChatClient
{
    /// <summary>
    /// This class is what contains the main execution method.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The main execution method that is called when the executable is run.
        /// </summary>
        /// <param name="args">The command line arguments. args[0] contains the IP address of the server. args[1] contains the username that the client will use.</param>
        static void Main(string[] args)
        {
            //Creates a new instance of the class that manages the connection to the server
            ClientHandler c = new ClientHandler(args[0], 13000, args[1]);

            //A try-catch loop is used to make sure that IO exceptions are properly handled.
            try 
            { 
                //A while(true) loop awaits for the Console.ReadLine() and sends the message to the server as soon as it's available.
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
