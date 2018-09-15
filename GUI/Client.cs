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
        public Client(MainWindow mainwindow)
        {
            mainWindow = mainwindow;
            MessageReceived += mainWindow.MessageReceived;
        }

        public Client(MainWindow mainwindow, string ipaddress, int port) : this(mainwindow)
        {
            ip = ipaddress;
            this.port = port;
        }

        public bool TryConnect()
        {
            try
            {
                transfer = new Transfer<MessageContainer>(new TcpClient(ip, port));
            }
            catch (SocketException)
            {
                return false;
            }
            return true;
        }

        public void Connect()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    while (transfer.Client.Connected)
                    {
                        try
                        {
                            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(transfer.Receive()));
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Server disconnected!");
                            mainWindow.Connected = false;
                        }
                        catch (InvalidOperationException)
                        {
                            Console.WriteLine("Server disconnedted???");
                            mainWindow.Connected = false;
                        }
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("Server disconnected!");
                    mainWindow.Connected = false;
                }
            });
        }

        public void Send(MessageContainer msgc)
        {
            try
            {
                transfer.Send(msgc);
            }
            catch (IOException)
            {
                Console.WriteLine("Server disconnected!");
                mainWindow.Connected = false;
            }
        }

        public void Send(Message msg)
        {
            Send(new MessageContainer() { Messages = new List<Message>() { msg } });
        }

        private string ip;
        private int port;
        private MainWindow mainWindow;
        private Transfer<MessageContainer> transfer;
        public event MessageReceivedEventHandler MessageReceived;
    }
}
