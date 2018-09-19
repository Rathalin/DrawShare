using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication.MessageTypes
{
    public class TextMessage : Message
    {
        public TextMessage()
        {
            Text = "";
        }

        public TextMessage(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
