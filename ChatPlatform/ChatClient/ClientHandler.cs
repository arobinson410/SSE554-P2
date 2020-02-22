using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChatClient
{
    public class ClientHandler
    {

        private TcpClient client;
        private NetworkStream stream;
        private string username;

        public ClientHandler(string address, Int32 port, string username)
        {
            client = new TcpClient(address, port);
            stream = client.GetStream();
            this.username = username;     

            SendLoginMessage();
            //Spawn a thread to wait for messages
            Thread t = new Thread(new ThreadStart(ReceiveMessageFromServer));
            t.Start();
        }

        ~ClientHandler()
        {
            StopClient();
        }

        public void SendLoginMessage()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(username + ":" + "" + "//" + MESSAGE_TYPE.LOGIN);
            stream.Write(data, 0, data.Length);
        }

        public void ReceiveMessageFromServer()
        {
            byte[] buf = new byte[256];
            while(true)
            {
                if(stream.Read(buf,0,buf.Length)!= 0)
                {
                    Console.WriteLine(Encoding.ASCII.GetString(buf));
                }
            }
        }

        public void SendMessage(string s)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(username + ":" + s + "//" + MESSAGE_TYPE.MESSAGE_SENT);
            stream.Write(data, 0, data.Length);
        }


        
        public void StopClient()
        {
            stream.Close();
            client.Close();
        }

    }
}
