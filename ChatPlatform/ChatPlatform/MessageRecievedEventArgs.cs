using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPlatform
{
    /// <summary>
    /// There are three kinds of messages sent to the server. Each one is encoded in the string sent to the server.
    /// </summary>
    public enum MESSAGE_TYPE
    {
        /// <summary>
        /// Login message tells the server the username of the new client.
        /// </summary>
        LOGIN,
        /// <summary>
        /// Lets the server know that the client is disconnecting.
        /// </summary>
        DISCONNECT,
        /// <summary>
        /// Lets the server know that a message intended for rebroadcast has been sent.
        /// </summary>
        MESSAGE_SENT,
    };

    /// <summary>
    /// This EventArgs class is derived so that the client can trigger an event containing a username, message type, and message.
    /// </summary>
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
