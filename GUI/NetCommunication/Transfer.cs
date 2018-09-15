using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GUI.NetCommunication
{
    public class Transfer<T>
    {
        public Transfer(TcpClient client)
        {
            Client = client;
            writer = new StreamWriter(client.GetStream());
            reader = new StreamReader(client.GetStream());
        }

        public TcpClient Client { get; set; }
        private XmlSerializer xmlS = new XmlSerializer(typeof(T));
        private StreamWriter writer;
        private StreamReader reader;

        public void Send(T t)
        {
            xmlS.Serialize(writer, t);
            writer.WriteLine();
            writer.Flush();
        }

        public T Receive()
        {
            string lines = "";
            string line = reader.ReadLine();
            while (line != "</" + typeof(T).Name + ">")
            {
                lines += line + "\n";
                line = reader.ReadLine();
            }
            lines += line + "\n";
            return (T)xmlS.Deserialize(new StringReader(lines));
        }
    }
}
