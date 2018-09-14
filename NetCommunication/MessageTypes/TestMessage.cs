﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCommunication.MessageTypes
{
    public class TestMessage : Message
    {
        public TestMessage() { }

        public TestMessage(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
