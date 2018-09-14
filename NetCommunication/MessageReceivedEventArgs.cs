using NetCommunication.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCommunication
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(MessageContainer msg)
        {
            MessageContainer = msg;
        }

        public MessageContainer MessageContainer { get; set; }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
}
