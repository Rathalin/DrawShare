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
            try
            {
                while (transfer.TcpClient.Connected)
                {
                    try
                    {
                        MessageReceived?.Invoke(null, new MessageReceivedEventArgs(transfer.Receive()));
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
        }

        public void Send(MessageContainer msgc)
        {
            try
            {
                transfer.Send(msgc);
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

        private void OnServerDisconnect()
        {
            Console.WriteLine("Server disconnected!");
            MessageReceived?.Invoke(null, new MessageReceivedEventArgs(new MessageContainer(new DrawUnlock())));
            mainWindow.Connected = false;
            mainWindow.UserCount = 1;
            mainWindow.IP = "";
            mainWindow.UseDispatcher(mainWindow, delegate
            {
                mainWindow.TBl_Info_Port.Text = "";
                mainWindow.Btn_LockDrawing.IsEnabled = true;
            });
        }

        private string ip;
        private int port;
        private MainWindow mainWindow;
        private Transfer<MessageContainer> transfer;
        public event MessageReceivedEventHandler MessageReceived;
    }
}
