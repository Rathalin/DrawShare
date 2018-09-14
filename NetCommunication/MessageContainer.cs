﻿using NetCommunication.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetCommunication
{
    public class MessageContainer
    {
        public MessageContainer()
        {
            Messages = new List<Message>();
        }

        public MessageContainer(List<Message> messages)
        {
            Messages = messages;
        }

        [XmlElement(typeof(TestMessage), ElementName = "TestMessage")]
        [XmlElement(typeof(DrawData), ElementName = "DrawData")]
        [XmlElement(typeof(DrawClear), ElementName = "DrawClear")]
        [XmlElement(typeof(DrawLock), ElementName = "DrawLock")]
        [XmlElement(typeof(DrawUnlock), ElementName = "DrawUnlock")]
        [XmlElement(typeof(UserCount), ElementName = "UserCount")]        
        public List<Message> Messages { get; set; }
    }
}