using GUI.Dialogs;
using GUI.Visualisation;
using NetCommunication;
using NetCommunication.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitHyperlinks();
            InitImages();
            InitValues();
        }

        #endregion Constructors

        #region Attributes and Variables

        private Server server;
        private Client client;
        private bool servermode;

        private CustomBrush CustomBrush = new CustomBrush(Brushes.Black, 1);

        private bool mouseDrawing = false;
        private Point mouseLastPosition;

        private bool _drawingLocked = true;
        public bool DrawingLocked
        {
            get { return _drawingLocked; }
            set
            {
                _drawingLocked = value;
                UseDispatcher(Btn_LockDrawing, delegate
                {
                    if (value)
                        Btn_LockDrawing.Background = ImageResource.PadlockClosed3;
                    else
                        Btn_LockDrawing.Background = ImageResource.PadlockOpen3;
                });

            }
        }

        private string _ip;
        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                UseDispatcher(TBl_Info_IP, delegate
                {
                    TBl_Info_IP.Text = value;
                });
            }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                UseDispatcher(TBl_Info_Port, delegate
                {
                    TBl_Info_Port.Text = value.ToString();
                });
            }
        }

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                UseDispatcher(SP_Status, delegate
                {
                    if (value)
                    {
                        TBl_ConnectionStatus.Text = "Verbunden";
                        B_ConnectionStatus.Background = Brushes.Green;
                        MI_Connect.IsEnabled = false;
                    }
                    else
                    {
                        TBl_ConnectionStatus.Text = "Getrennt";
                        B_ConnectionStatus.Background = Brushes.Red;
                        MI_Connect.IsEnabled = true;
                    }
                });
                _connected = value;
            }
        }

        private int _userCount;
        public int UserCount
        {
            get { return _userCount; }
            set
            {
                UseDispatcher(TBl_UserCount, delegate
                {
                    TBl_UserCount.Text = value.ToString();
                });
                _userCount = value;
            }
        }

        #endregion Attributes and Variables

        #region Methodes

        private void DrawLine(Canvas target, CustomBrush brush, double x1, double y1, double x2, double y2)
        {
            target.Children.Add(new Line()
            {
                Stroke = brush.ColorBrush,
                StrokeThickness = brush.Thickness,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            });
        }

        private void DrawEllipse(Canvas target, CustomBrush brush, double x1, double y1)
        {
            Ellipse ell = new Ellipse()
            {
                Stroke = brush.ColorBrush,
                StrokeThickness = brush.Thickness,
                Width = brush.Thickness,
                Height = brush.Thickness
            };
            ell.SetValue(Canvas.LeftProperty, x1 - brush.Thickness / 2);
            ell.SetValue(Canvas.TopProperty, y1 - brush.Thickness / 2);
            Canvas_Drawing.Children.Add(ell);
        }

        private void Draw(Canvas target, CustomBrush brush, double x1, double y1, double x2, double y2)
        {
            DrawEllipse(target, brush, x1, y1);
            DrawLine(target, brush, x1, y1, x2, y2);
        }

        public void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            foreach (var m in e.MessageContainer.Messages)
            {
                if (m as DrawData != null)
                {
                    DrawData drawData = (DrawData)m;
                    if (servermode)
                    {
                        server.SendAll(drawData);
                    }
                    UseDispatcher(Canvas_Drawing, delegate
                    {
                        Draw(Canvas_Drawing,
                            new CustomBrush(drawData.Color, drawData.Thickness), drawData.X1, drawData.Y1, drawData.X2, drawData.Y2);
                    });
                }
                else if (m as DrawClear != null)
                {
                    UseDispatcher(Canvas_Drawing, delegate
                    {
                        Canvas_Drawing.Children.Clear();
                    });
                }
                //client only receives
                else if (!servermode)
                {
                    if (m as DrawLock != null)
                    {
                        DrawingLocked = true;
                        UseDispatcher(SP_Board, delegate { SP_Board.IsEnabled = false; });
                        UseDispatcher(TBl_ControlPanel, delegate { TBl_ControlPanel.Text = "Der Host hat das Zeichenbrett gesperrt!"; });
                    }
                    else if (m as DrawUnlock != null)
                    {
                        DrawingLocked = false;
                        UseDispatcher(SP_Board, delegate { SP_Board.IsEnabled = true; });
                        UseDispatcher(TBl_ControlPanel, delegate { TBl_ControlPanel.Text = string.Empty; });
                    }
                    else if (m as UserCount != null)
                    {
                        UserCount userC = (UserCount)m;
                        UserCount = userC.Count;
                    }
                }
            }
        }

        public void UseDispatcher(UIElement el, Action func)
        {
            el.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new DispatcherOperationCallback(delegate
                {
                    func();
                    return null;
                }),
                null
            );
        }

        private void Send(MessageContainer msg)
        {
            if (servermode)
                server.SendAll(msg);
            else
                client.Send(msg);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetGlobalIPAddress()
        {
            return new WebClient().DownloadString("http://icanhazip.com").Trim();
        }

        private void SetTheme(Color colorBegin, Color colorEnd, Color colorBeginDark, Color colorEndDark)
        {
            try
            {
                Application.Current.Resources["ColorBegin_Standard"] = colorBegin;
                Application.Current.Resources["ColorEnd_Standard"] = colorEnd;
                Application.Current.Resources["ColorBeginDark_Standard"] = colorBeginDark;
                Application.Current.Resources["ColorEndDark_Standard"] = colorEndDark;
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                MessageBox.Show("Theme konnte nicht gefunden werden. Bitte wenden sie sich an den schlechten Programmierer!");
            }
        }

        private void InitHyperlinks()
        {
            /*
             * Hyperlink_Twitter.RequestNavigate += (sender, e) =>
            {
                System.Diagnostics.Process.Start(e.Uri.ToString());
            };
            */
            Btn_Twitter.Tag = "https://twitter.com/Rhatalin";
            Btn_Facebook.Tag = "https://www.facebook.com/daniel.flockert";
            Btn_Instagram.Tag = "https://www.instagram.com/rhatali/";

        }

        private void InitImages()
        {
            //Icon
            Icon = ImageResource.DrawShareLogo1;

            //Usericon
            Label_UserCount.Background = ImageResource.UserCount;

            //Clear
            Btn_Clear.Background = ImageResource.Trash1_Small;


            //Twitter
            Btn_Twitter.Background = ImageResource.LogoTwitter;
            //Facebook
            Btn_Facebook.Background = ImageResource.LogoFacebook;
            //Instagram
            Btn_Instagram.Background = ImageResource.LogoInstagram;
        }

        private void InitValues()
        {
            //Lock
            DrawingLocked = true;
            Btn_LockDrawing.IsEnabled = false;

            //UserCount
            UserCount = 1;
        }

        #endregion Methodes

        #region GUI

        private void Canvas_Drawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDrawing)
            {
                Point position = Mouse.GetPosition(Canvas_Drawing);
                if (mouseLastPosition.X == -1 && mouseLastPosition.Y == -1)
                {
                    mouseLastPosition = position;
                }
                else
                {
                    Draw(Canvas_Drawing, CustomBrush, mouseLastPosition.X, mouseLastPosition.Y, position.X, position.Y);
                    if (Connected)
                    {
                        Send(new MessageContainer()
                        {
                            Messages = new List<Message>() {
                                new DrawData(mouseLastPosition.X, mouseLastPosition.Y, position.X, position.Y, CustomBrush.Thickness, CustomBrush.ColorBrush.ToString())
                            }
                        });
                    }
                    mouseLastPosition = Mouse.GetPosition(Canvas_Drawing);
                }
            }
        }

        private void Canvas_Drawing_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDrawing = true;
            mouseLastPosition = Mouse.GetPosition(Canvas_Drawing);
            DrawEllipse(Canvas_Drawing, CustomBrush, mouseLastPosition.X, mouseLastPosition.Y);
        }

        private void Canvas_Drawing_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDrawing = false;
        }

        private void Canvas_Drawing_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseLastPosition.X = -1;
            mouseLastPosition.Y = -1;
        }

        private void Canvas_Drawing_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                mouseDrawing = false;
            }
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            Canvas_Drawing.Children.Clear();
            if (servermode)
            {
                server.SendAll(new DrawClear());
            }
            else if (client != null && !DrawingLocked)
            {
                client.Send(new DrawClear());
            }
        }



        private void Btn_Thickness_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                Shape shape = (Shape)btn.Content;
                CustomBrush.Thickness = (double)shape.GetValue(HeightProperty);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("InvalidCastException");
            }
        }

        private void Btn_ColorPicker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                CustomBrush.ColorBrush = btn.GetValue(BackgroundProperty) as SolidColorBrush;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("InvalidCastException");
            }
        }

        private void MenuItem_Restart_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void MenuItem_Client_Click(object sender, RoutedEventArgs e)
        {
            if (server == null && client == null)
            {
                servermode = false;
                DialogChangeConnection dlg = new DialogChangeConnection("10.0.0.1", 59595);
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    Title = "DrawShare Client";
                    client = new Client(this, dlg.IPAddress, dlg.Port);
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        bool connected = client.TryConnect();
                        if (connected)
                        {
                            client.Connect();
                            UseDispatcher(this, delegate
                            {
                                IP = dlg.IPAddress;
                                Port = dlg.Port;
                                Connected = true;
                                Canvas_Drawing.Children.Clear();
                                MI_Share.IsEnabled = false;
                            });
                        }
                        else
                        {
                            MessageBox.Show("Es konnte keine Verbindung zum Partner hergestellt werden!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            client = null;
                        }
                    });
                }
            }
        }

        private void MenuItem_Server_Click(object sender, RoutedEventArgs e)
        {
            int port = 59595;
            if (client == null)
            {
                servermode = true;
                if (server == null)
                {
                    Title = "DrawShare Server";
                    server = new Server(this, port);
                    server.Listen();
                    Connected = true;
                    Btn_LockDrawing.IsEnabled = true;
                }
                IP = GetGlobalIPAddress();
                Port = port;
                DialogConnectionInfo dlg = new DialogConnectionInfo("Mit diesen Daten können sich andere mit dir verbinden.", IP, Port);
                dlg.Owner = this;
                dlg.Show();
            }
        }

        private void MI_Theme_Classic_Click(object sender, RoutedEventArgs e)
        {
            SetTheme(Colors.Gray, Colors.WhiteSmoke, Colors.Gray, Colors.Black);
        }

        private void MI_Theme_GreenBlue_Click(object sender, RoutedEventArgs e)
        {
            SetTheme(Colors.LightGreen, Colors.LightBlue, Colors.Green, Colors.DarkBlue);
        }


        private void MI_Debug_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_LockDrawing_Click(object sender, RoutedEventArgs e)
        {
            if (DrawingLocked)
            {
                if (servermode)
                    server.SendAll(new DrawUnlock());
            }
            else
            {
                if (servermode)
                    server.SendAll(new DrawLock());
            }
            DrawingLocked = !DrawingLocked;
        }

        private void Btn_SocialMedia_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            System.Diagnostics.Process.Start(btn.Tag.ToString());
        }
    }
    #endregion GUI
}
