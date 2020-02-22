using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public enum MESSAGE_TYPE
    {
        LOGIN,
        DISCONNECT,
        MESSAGE_SENT,
    };

    public class MessageRecievedEventArgs : EventArgs
    {
        public string message;

        public MessageRecievedEventArgs(string message)
        {
            this.message = message;
        }

    }
}
