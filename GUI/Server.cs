using GUI.NetCommunication;
using GUI.NetCommunication.MessageTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GUI
{
    public class Server
    {
        public Server(MainWindow mainwindow)
        {
            mainWindow = mainwindow;
            MessageReceived += mainWindow.MessageReceived;
        }

        public bool TryPort(int port)
        {
            try
            {
                Listener = new TcpListener(IPAddress.Any, port);
                Listener.Start();
            }
            catch (SocketException)
            {
                return false;
            }
            return true;
        }

        public void Receive()
        {
            try
            {
                while (true)
                {
                    Transfer<MessageContainer> t = new Transfer<MessageContainer>(Listener.AcceptTcpClient());
                    //MessageBox.Show("Client connected");
                    Transfers.Add(t);
                    mainWindow.UseDispatcher(mainWindow.TBl_UserCount, delegate { mainWindow.TBl_UserCount.Text = (Transfers.Count + 1).ToString(); });
                    List<Message> msgList = new List<Message>();
                    mainWindow.UseDispatcher(mainWindow.Canvas_Drawing, delegate
                    {
                    //Send init data (all in this thread to avoid race conditions)
                    foreach (var el in mainWindow.Canvas_Drawing.Children)
                        {
                            if (el as Line != null)
                            {
                                Line l = (Line)el;
                                msgList.Add(new DrawData(l.X1, l.Y1, l.X2, l.Y2, (double)l.GetValue(Shape.StrokeThicknessProperty), l.GetValue(Shape.StrokeProperty).ToString()));
                            }
                        }
                        if (mainWindow.DrawingLocked)
                            msgList.Add(new DrawLock());
                        else
                            msgList.Add(new DrawUnlock());
                        t.Send(new MessageContainer() { Messages = msgList });

                    //Update other users
                    SendAll(new UserCount(Transfers.Count + 1));
                    });

                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        try
                        {
                            while (true)
                            {
                                MessageReceived?.Invoke(null, new MessageReceivedEventArgs(t.Receive()));
                            }
                        }
                        catch (IOException)
                        {
                        //MessageBox.Show("Client disconnected");
                        Transfers.Remove(t);
                            mainWindow.UserCount = Transfers.Count + 1;
                            SendAll(new UserCount(Transfers.Count + 1));
                        }
                    });
                }
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode != 10004)
                {
                    throw ex;
                }
            }
        }

        public void Send(Transfer<MessageContainer> transfer, MessageContainer msgc)
        {
            try
            {
                transfer.Send(msgc);
            }
            catch (IOException)
            {
                Console.WriteLine("Client disconnected!");
                Transfers.Remove(transfer);
            }
        }

        public void Send(Transfer<MessageContainer> transfer, Message msg)
        {
            Send(transfer, new MessageContainer() { Messages = new List<Message>() { msg } });
        }

        public void SendAll(MessageContainer msgc)
        {
            foreach (var t in Transfers)
                t.Send(msgc);
        }

        public void SendAll(Message msg)
        {
            foreach (var t in Transfers)
                t.Send(new MessageContainer() { Messages = new List<Message>() { msg } });
        }

        public void Stop()
        {
            for (int i = 0; i < Transfers.Count; i++)
                Transfers[i] = null;
            Listener.Stop();
            Transfers.Clear();
        }

        public TcpListener Listener { get; set; }
        public List<Transfer<MessageContainer>> Transfers { get; set; } = new List<Transfer<MessageContainer>>();
        public event MessageReceivedEventHandler MessageReceived;
        private MainWindow mainWindow;
    }
}
