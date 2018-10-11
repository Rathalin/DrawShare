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
using System.Windows;
using static GUI.MainWindow;
using System.Windows.Threading;
using static GUI.Enums;

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

        private string ip;
        private int port;
        private MainWindow mw;
        public Transfer<MessageContainer> Transfer { get; set; }
        public event MessageReceivedEventHandler MessageReceived;

        public bool TryConnect()
        {
            try
            {
                Transfer = new Transfer<MessageContainer>(new TcpClient(ip, port));
            }
            catch (SocketException)
            {
                mw.WriteDebug("SocketException in Client.TryConnect", LogLevels.Debug);
                return false;
            }
            return true;
        }

        public void Connect()
        {
            try
            {
                while (Transfer != null && Transfer.TcpClient.Connected)
                {
                    try
                    {
                        MessageReceived?.Invoke(null, new MessageReceivedEventArgs(Transfer.Receive()));
                    }
                    catch (IOException) //Server gone without sending ServerDisconnect-Message
                    {
                        OnServerDisconnect();
                        mw.WriteDebug("IOException in Client.Connect", LogLevels.Debug);
                    }
                    catch (InvalidOperationException)
                    {
                        mw.WriteDebug("InvalidOperationException in Client.Connect", LogLevels.Debug);
                    }
                }
            }
            catch (SocketException)
            {
                OnServerDisconnect();
                mw.WriteDebug("SocketException in Client.Connect", LogLevels.Debug);
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
                mw.WriteDebug("IOException in Client.Send", LogLevels.Debug);
            }
        }

        public void Send(Message msg)
        {
            Send(new MessageContainer(msg));
        }

        public void Stop()
        {
            if (Transfer != null)
            {
                Send(new MessageContainer(new ClientDisconnect()));
                Transfer.TcpClient.Close();
                Transfer = null;
            }
        }

        public void OnServerDisconnect()
        {
            mw.UseDispatcher(mw, DispatcherPriority.Send, delegate
            {
                mw.WriteDebug("Server disconnected!", LogLevels.Info);
                mw.ResetConnectionBar();
                mw.SP_Board.IsEnabled = true;
                mw.Btn_LockDrawing.IsEnabled = true;
                mw.TBl_ControlPanel.Text = "";
            });
        }
    }
}
