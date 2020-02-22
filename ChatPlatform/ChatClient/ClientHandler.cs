using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
