using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using GUI.NetCommunication;
using GUI.NetCommunication.MessageTypes;

namespace GUI
{
    public class Client
    {
        public Client(MainWindow mainWindow)
        {
            mw = mainWindow;
            MessageReceived += mw.MessageReceived;
        }

        public Client(MainWindow mw, string ipaddress, int port) : this(mw)
        {
            ip = ipaddress;
            this.port = port;
        }

        public bool TryConnect()
        {
            try
            {
                Transfer = new Transfer<MessageContainer>(new TcpClient(ip, port));
            }
            catch (SocketException)
            {
                return false;
            }
            return true;
        }

        public void Connect()
        {
            try
            {
                while (Transfer.TcpClient.Connected)
                {
                    try
                    {
                        MessageReceived?.Invoke(null, new MessageReceivedEventArgs(Transfer.Receive()));
                    }
                    catch (IOException)
                    {
                        OnServerDisconnect();
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Server disconnedted???");
                    }
                }
            }
            catch (SocketException)
            {
                OnServerDisconnect();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Client stopped");
            }
        }

        public void Send(MessageContainer msgc)
        {
            try
            {
                Transfer.Send(msgc);
            }
            catch (IOException)
            {
                OnServerDisconnect();
            }
        }

        public void Send(Message msg)
        {
            Send(new MessageContainer() { Messages = new List<Message>() { msg } });
        }

        public void Stop()
        {
            Transfer = null;
        }

        private void OnServerDisconnect()
        {
            Console.WriteLine("Server disconnected!");
            mw.UseDispatcher(mw, delegate { mw.Reset(); });
        }

        private string ip;
        private int port;
        private MainWindow mw;
        public Transfer<MessageContainer> Transfer { get; private set; }
        public event MessageReceivedEventHandler MessageReceived;
    }
}
