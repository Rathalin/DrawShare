using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication.MessageTypes
{
    public class TestMessage : Message
    {
        public TestMessage()
        {
            Text = "";
        }

        public TestMessage(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
