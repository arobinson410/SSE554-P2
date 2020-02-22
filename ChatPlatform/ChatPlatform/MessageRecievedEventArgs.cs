using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPlatform
{
    public enum MESSAGE_TYPE
    {
        LOGIN,
        DISCONNECT,
        MESSAGE_SENT,
    };

    public class MessageRecievedEventArgs: EventArgs
    {
        public string sender;
        public string message;
        public MESSAGE_TYPE t;

        public MessageRecievedEventArgs(string sender, MESSAGE_TYPE t, string message)
        {
            this.sender = sender;
            this.message = message;
            this.t = t;
        }

    }
}
