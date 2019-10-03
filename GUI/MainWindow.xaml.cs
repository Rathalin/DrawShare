using GUI.Dialogs;
using GUI.LanguagePacks;
using GUI.Visualisation;
using GUI.NetCommunication;
using GUI.NetCommunication.MessageTypes;
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
using System.Diagnostics;
using static GUI.Enums;
using System.Text.RegularExpressions;

namespace GUI
{
    /// <summary>
    /// MainWindow for GUI
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitHyperlinks();
            InitImages();
            InitValues();
            Translation = Languages.English;
            ToolTipService.SetShowOnDisabled(Btn_LockDrawing, true);

            DebugLevel = LogLevels.NoLog;

            foreach (FontFamily ff in Fonts.SystemFontFamilies)
            {
                InstalledFontFamilies.Add(ff.ToString());
            }
        }

        #endregion Constructors

        #region Attributes and Variables

        public static List<string> InstalledFontFamilies = new List<string>();
        public static Random Random = new Random();

        private Thread stopwatchLoading;

        private Server server;
        private Client client;
        private const int startport = 50000;

        private CustomBrush CustomBrush = new CustomBrush(Brushes.Black, 2);

        private bool mouseDrawing = false;
        private Point mouseLastPosition;
        private Point mouseLastEllipsePosition;

        public Modes ApplicationMode { get; set; } = Modes.Undefined;
        public LogLevels DebugLevel { get; set; } = LogLevels.Debug;

        private bool _drawingLocked;
        public bool DrawingLocked
        {
            get { return _drawingLocked; }
            set
            {
                _drawingLocked = value;
                UseDispatcher(Btn_LockDrawing, delegate
                {
                    if (value)
                        Btn_LockDrawing.Background = ImageResource.PadlockClosed;
                    else
                        Btn_LockDrawing.Background = ImageResource.PadlockOpen;
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
                    if (value > 0)
                        TBl_Info_Port.Text = value.ToString();
                    else
                        TBl_Info_Port.Text = "";
                });
            }
        }

        private ConnectionStatus _connectionState;
        public ConnectionStatus ConnectionState
        {
            get { return _connectionState; }
            set
            {
                UseDispatcher(SP_Status, delegate
                {
                    _connectionState = value;
                    switch (value)
                    {
                        case ConnectionStatus.Disconnected:
                            TBl_ConnectionStatus.Text = Translation.Connection_Status_Disconnected;
                            B_ConnectionStatus.Background = Brushes.Red;
                            break;
                        case ConnectionStatus.ClientConnecting:
                            TBl_ConnectionStatus.Text = Translation.Connection_Status_ClientConnecting;
                            B_ConnectionStatus.Background = Brushes.LightBlue;
                            break;
                        case ConnectionStatus.ClientConnected:
                            TBl_ConnectionStatus.Text = Translation.Connection_Status_ClientConnected;
                            B_ConnectionStatus.Background = Brushes.LightGreen;
                            break;
                        case ConnectionStatus.ServerConnecting:
                            TBl_ConnectionStatus.Text = Translation.Connection_Status_ServerConnecting;
                            B_ConnectionStatus.Background = Brushes.Blue;
                            break;
                        case ConnectionStatus.ServerOnline:
                            TBl_ConnectionStatus.Text = Translation.Connection_Status_ServerOnline;
                            B_ConnectionStatus.Background = Brushes.Green;
                            break;
                    }
                });
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

        private Language _translation;
        public Language Translation
        {
            get { return _translation; }
            private set
            {
                if (_translation != value)
                {
                    _translation = value;
                    UseDispatcher(this, delegate
                    {
                        MI_Menu.Header = _translation.MenuBar_Menu;
                        MI_Theme.Header = _translation.MenuBar_Menu_Theme;
                        MI_Theme_Classic.Header = _translation.MenuBar_Menu_Theme_Classic;
                        MI_Theme_GreenBlue.Header = _translation.MenuBar_Menu_Theme_GreenBlue;
                        MI_Language.Header = _translation.MenuBar_Language;
                        MI_Language_English.Header = _translation.MenuBar_Language_English;
                        MI_Language_German.Header = _translation.MenuBar_Language_German;
                        MI_ReportBug.Header = _translation.MenuBar_Menu_ReportBug;
                        MI_Exit.Header = _translation.MenuBar_Menu_Exit;
                        MI_Share.Header = _translation.MenuBar_Share;
                        MI_Join.Header = _translation.MenuBar_Join;
                        TBl_IP.Text = _translation.Connection_TBl_IP;
                        TBl_Port.Text = _translation.Connection_TBl_Port;
                        ConnectionState = _connectionState;
                        Btn_Clear.ToolTip = _translation.PaintMenu_Clear_Tooltip;
                        Btn_LockDrawing.ToolTip = _translation.ControlMenu_Lock_Tooltip;
                        if (TBl_ControlPanel.Text != "")
                            TBl_ControlPanel.Text = _translation.ControlMenu_Lock_Text;
                    });
                }
            }
        }

        #endregion Attributes and Variables

        #region Methodes

        private void InitImages()
        {
            //Icon
            Icon = ImageResource.DrawShareLogo;

            //Usericon
            Label_UserCount.Background = ImageResource.UserCount;

            //Clear
            Btn_Clear.Background = ImageResource.Trash;

            //Twitter
            Btn_Twitter.Background = ImageResource.LogoTwitter;
            //Github
            Btn_Github.Background = ImageResource.LogoGithub;
        }

        private void InitValues()
        {
            //Lock
            DrawingLocked = false;

            //UserCount
            UserCount = 1;
        }

        private void InitHyperlinks()
        {
            string linkTwitter = "https://twitter.com/Rhatalin";
            Btn_Twitter.Tag = linkTwitter;
            Btn_Twitter.ToolTip = linkTwitter;

            string linkInstagram = "https://github.com/Rhatalin";
            Btn_Github.Tag = linkInstagram;
            Btn_Github.ToolTip = linkInstagram;

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

        public void UseDispatcher(UIElement el, DispatcherPriority priority, Action func)
        {
            el.Dispatcher.BeginInvoke(
                priority,
                new DispatcherOperationCallback(delegate
                {
                    func();
                    return null;
                }),
                null
            );
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
            string ip = "";
            string response = new WebClient().DownloadString(Constants.DNS_GetIP_URI).Trim();
            Regex reg = new Regex(Constants.RegexIP);
            Match match = reg.Match(response);
            if (match.Success)
            {
                ip = match.Value;
            }
            else
            {
                throw new Exception("Failed to parse Web Response of " + Constants.DNS_GetIP_URI);
            }
            return ip;
        }

        public void OpenURI(string uri)
        {
            System.Diagnostics.Process.Start(uri);
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
                WriteDebug("ResourceReferenceKeyNotFoundException in SetTheme", LogLevels.Error);
            }
        }

        private void Send(MessageContainer msg)
        {
            if (ApplicationMode == Modes.Server)
                server.SendAll(msg);
            else if (ApplicationMode == Modes.Client)
                client.Send(msg);
        }

        public void WriteDebug(string text, LogLevels loglvl)
        {
            if (loglvl <= DebugLevel)
            {
                UseDispatcher(TBl_Debug, delegate
                {
                    TBl_Debug.Text = string.Format("{0}: {1}", loglvl, text);
                });
                Console.WriteLine(string.Format("{0}: {1}", loglvl, text));
            }
        }

        private void StartLoading()
        {
            UseDispatcher(SP_Board, DispatcherPriority.Normal, delegate
            {
                TextBlock tbl = new TextBlock();
                tbl.Style = FindResource("Style_TextBlock_Loading") as Style;
                tbl.SetValue(Canvas.LeftProperty, Canvas_Drawing.ActualWidth / 2 - tbl.Width / 2);
                tbl.SetValue(Canvas.TopProperty, Canvas_Drawing.ActualHeight / 2 - tbl.Height / 2);
                Canvas_Drawing.Children.Add(tbl);
                SP_Board.IsEnabled = false;
                stopwatchLoading = new Thread(new ThreadStart(delegate
                {
                    int i = 0;
                    while (true)
                    {
                        UseDispatcher(tbl, delegate
                        {
                            tbl.Text = string.Format("Loading({0}s) ...", i);
                            i++;
                        });
                        Thread.Sleep(1000);
                    }
                }));
                stopwatchLoading.Start();
            });
        }

        private void StopLoading()
        {
            stopwatchLoading.Abort();
            UseDispatcher(Canvas_Drawing, delegate
            {
                foreach (UIElement el in Canvas_Drawing.Children)
                {
                    if (el as TextBlock != null)
                    {
                        Canvas_Drawing.Children.Remove(el);
                        break;
                    }
                }
            });
            UseDispatcher(this, delegate
            {
                SP_Board.IsEnabled = true;
            });
        }

        private void CleanUp()
        {
            if (stopwatchLoading != null)
            {
                stopwatchLoading.Abort();
            }
        }

        // MESSAGE RECEIVED
        public void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            foreach (var m in e.MessageContainer.Messages)
            {
                if (m as DrawData != null)
                {
                    DrawData drawData = (DrawData)m;
                    if (ApplicationMode == Modes.Server)
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
                    if (ApplicationMode == Modes.Server)
                        server.SendAll(m);
                }

                //Client only receives
                else if (ApplicationMode == Modes.Client)
                {
                    if (m as UserCount != null)
                    {
                        UserCount userC = (UserCount)m;
                        UserCount = userC.Count;
                    }
                    else if (m as DrawLock != null)
                    {
                        UseDispatcher(SP_Board, DispatcherPriority.Send, delegate { SP_Board.IsEnabled = false; });
                        DrawingLocked = true;
                        UseDispatcher(TBl_ControlPanel, delegate { TBl_ControlPanel.Text = Translation.ControlMenu_Lock_Text; });
                    }
                    else if (m as DrawUnlock != null)
                    {
                        UseDispatcher(SP_Board, DispatcherPriority.Send, delegate { SP_Board.IsEnabled = true; });
                        DrawingLocked = false;
                        UseDispatcher(TBl_ControlPanel, delegate { TBl_ControlPanel.Text = string.Empty; });
                    }
                    else if (m as DrawDataBlockFlag != null)
                    {
                        StartLoading();
                        UseDispatcher(TBl_ControlPanel, delegate { WriteDebug("DrawDataBlock Flag", LogLevels.Debug); });
                    }
                    else if (m as DrawDataBlock != null)
                    {
                        DrawDataBlock drawDataBlock = (DrawDataBlock)m;
                        if (ApplicationMode == Modes.Server)
                        {
                            server.SendAll(drawDataBlock);
                        }
                        StopLoading();
                        UseDispatcher(SP_Board, delegate
                        {
                            foreach (var line in drawDataBlock.Lines)
                                Draw(Canvas_Drawing,
                                    new CustomBrush(drawDataBlock.Color, drawDataBlock.Thickness), line.X1, line.Y1, line.X2, line.Y2);
                        });
                        UseDispatcher(TBl_ControlPanel, delegate { WriteDebug("DrawDataBlock", LogLevels.Debug); });
                    }
                    else if (m as ServerDisconnect != null)
                    {
                        client.OnServerDisconnect();
                    }
                }

                //Server only receives
                else if (ApplicationMode == Modes.Server)
                {
                    if (m as ClientDisconnect != null)
                    {
                        server.OnClientDisconnect((Transfer<MessageContainer>)sender);
                    }
                }
            }
        }

        public void ResetConnections()
        {
            if (client != null)
                client.Stop();
            if (server != null)
                server.Stop();
        }

        public void ResetConnectionBar()
        {
            ApplicationMode = Modes.Undefined;
            ConnectionState = ConnectionStatus.Disconnected;
            IP = "";
            Port = 0;
            UserCount = 1;
        }

        #endregion Methodes

        #region GUI-Methodes

        #region GUI Drawing

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
            //Console.WriteLine(string.Format("{0}:{1} -> {2}:{3}", x1, y1, x2, y2));
        }

        private void DrawEllipse(Canvas target, CustomBrush brush, double x1, double y1)
        {
            mouseLastEllipsePosition.X = x1;
            mouseLastEllipsePosition.Y = y1;
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
            /*
            UseDispatcher(TBl_Debug, DispatcherPriority.Loaded, delegate
            {
                drawCount += 2;
                TBl_Debug.Text = drawCount.ToString();
            });
            */
        }

        private void Canvas_Drawing_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = Mouse.GetPosition(Canvas_Drawing);
            if (mouseDrawing)
            {
                if (mouseLastPosition.X == -1 && mouseLastPosition.Y == -1)
                {
                    mouseLastPosition = position;
                }
                else
                {
                    Draw(Canvas_Drawing, CustomBrush, mouseLastPosition.X, mouseLastPosition.Y, position.X, position.Y);
                    if (ConnectionState == ConnectionStatus.ClientConnected || ConnectionState == ConnectionStatus.ServerOnline)
                    {
                        Send(new MessageContainer(
                            new DrawData(mouseLastPosition.X, mouseLastPosition.Y, position.X, position.Y, CustomBrush.Thickness, CustomBrush.ColorBrush.ToString())
                        ));
                    }
                    mouseLastPosition = position;
                }
            }
        }

        private void Canvas_Drawing_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDrawing = true;
            Point currentPosition = Mouse.GetPosition(Canvas_Drawing);
            mouseLastPosition = currentPosition;
            if (mouseLastEllipsePosition.X != currentPosition.X && mouseLastEllipsePosition.Y != currentPosition.Y)
            {
                DrawEllipse(Canvas_Drawing, CustomBrush, mouseLastPosition.X, mouseLastPosition.Y);
                mouseLastEllipsePosition = currentPosition;
            }
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
            if (ApplicationMode == Modes.Server)
            {
                server.SendAll(new DrawClear());
            }
            else if (ApplicationMode == Modes.Client && ConnectionState == ConnectionStatus.ClientConnected && !DrawingLocked)
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
                WriteDebug("CustomBrush.Thickness: " + CustomBrush.Thickness.ToString(), LogLevels.Debug);
            }
            catch (InvalidCastException)
            {
                WriteDebug("InvalidCastException in Btn_Thickness_Click", LogLevels.Debug);
            }
        }

        private void Btn_ColorPicker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                CustomBrush.ColorBrush = btn.Background as SolidColorBrush;
            }
            catch (InvalidCastException)
            {
                WriteDebug("InvalidCastException in Btn_ColorPicker_Click", LogLevels.Debug);
            }
        }

        #endregion GUI Drawing

        private void MenuItem_Join_Click(object sender, RoutedEventArgs e)
        {
            ResetConnections();
            ResetConnectionBar();
            Btn_LockDrawing.IsEnabled = false;
            DialogChangeConnection dlg = new DialogChangeConnection(
                Translation.General_Connection, Translation.General_IP, "",
                Translation.General_Port, startport, Translation.General_Cancel, Translation.General_Connect, Translation.General_Paste,
                Translation.Dialog_ChangeConnection_InvalidIP, Translation.Dialog_ChangeConnection_InvalidPort, Translation.General_InvalidInput);
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                ApplicationMode = Modes.Client;
                client = new Client(this, dlg.IPAddress, dlg.Port);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    ConnectionState = ConnectionStatus.ClientConnecting;
                    UseDispatcher(MI_Join, delegate
                    {
                        MI_Join.IsEnabled = false;
                        MI_Share.IsEnabled = false;
                        IP = dlg.IPAddress;
                        Port = dlg.Port;
                    });
                    if (client.TryConnect())
                    {
                        ThreadPool.QueueUserWorkItem(delegate { client.Connect(); });
                        UseDispatcher(this, delegate
                        {
                            ConnectionState = ConnectionStatus.ClientConnected;
                            Canvas_Drawing.Children.Clear();
                            MI_Join.IsEnabled = true;
                            MI_Share.IsEnabled = true;
                        });
                    }
                    else
                    {
                        UseDispatcher(this, DispatcherPriority.Send, delegate
                        {
                            ResetConnectionBar();
                            MI_Join.IsEnabled = true;
                            MI_Share.IsEnabled = true;
                            MessageBox.Show(this, Translation.Dialog_ClientConnectionError_ErrorMsg, Translation.General_Error,
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No, MessageBoxOptions.None);
                        });
                    }
                });
            }
            else
            {
                ResetConnectionBar();
            }
        }

        private void MenuItem_Share_Click(object sender, RoutedEventArgs e)
        {
            //ResetConnections();
            if (client != null)
                client.Stop();

            ThreadPool.QueueUserWorkItem(delegate
            {
                UseDispatcher(MI_Join, delegate
                {
                    MI_Join.IsEnabled = false;
                    MI_Share.IsEnabled = false;
                    ConnectionState = ConnectionStatus.ServerConnecting;
                });
                if (ApplicationMode != Modes.Server)
                {
                    int port = startport;
                    server = new Server(this);
                    while (!server.TryPort(port))
                    {
                        if (port >= 65535)
                            port = 55550;
                        else
                            port++;
                    }
                    Port = port;
                    ThreadPool.QueueUserWorkItem(delegate { server.Receive(); });
                }
                string localIP = GetLocalIPAddress();
                string globalIP = "";
                bool localOnly = false;
                try
                {
                    globalIP = GetGlobalIPAddress();
                }
                catch (WebException)
                {
                    localOnly = true;
                    WriteDebug("WebException in MenuItem_Share_Click", LogLevels.Error);
                }
                if (localOnly)
                    MessageBox.Show(Translation.Dialog_ServerConnectionError_ErrorMsg, Translation.General_Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                UseDispatcher(this, DispatcherPriority.ApplicationIdle, delegate
                {
                    ConnectionState = ConnectionStatus.ServerOnline;
                    Btn_LockDrawing.IsEnabled = true;
                    Regex RegIP = new Regex(Constants.RegexIP);
                    if (RegIP.Match(globalIP).Success)
                    {
                        IP = globalIP;
                    }
                    else
                    {
                        IP = localIP;
                        globalIP = Translation.Connection_Status_Disconnected;
                    }
                    MI_Join.IsEnabled = true;
                    MI_Share.IsEnabled = true;
                    DialogConnectionInfo dlg = new DialogConnectionInfo(
                        Translation.General_Connection, Translation.Dialog_ConnectionInfo_Infotext,
                        Translation.General_IP, localIP, globalIP, Translation.General_Port, Port,
                        Translation.General_Copy, Translation.General_Close);
                    dlg.Owner = this;
                    dlg.ShowDialog();
                    ApplicationMode = Modes.Server;
                });
            });
        }

        private void Btn_LockDrawing_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (ApplicationMode == Modes.Server)
                {
                    if (DrawingLocked)
                        server.SendAll(new DrawUnlock());
                    else
                        server.SendAll(new DrawLock());
                }
                UseDispatcher(this, () => DrawingLocked = !DrawingLocked);
            });
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MI_Theme_Classic_Click(object sender, RoutedEventArgs e)
        {
            SetTheme(Colors.Gray, Colors.WhiteSmoke, Colors.Gray, Colors.Black);
        }

        private void MI_Theme_GreenBlue_Click(object sender, RoutedEventArgs e)
        {
            SetTheme(Colors.LightGreen, Colors.LightBlue, Colors.Green, Colors.DarkBlue);
        }

        private void MI_Language_English_Click(object sender, RoutedEventArgs e)
        {
            Translation = Languages.English;
        }

        private void MI_Language_German_Click(object sender, RoutedEventArgs e)
        {
            Translation = Languages.German;
        }

        private void Btn_SocialMedia_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            OpenURI(btn.Tag.ToString());
        }

        private void MenuItem_ReportBug_Click(object sender, RoutedEventArgs e)
        {
            OpenURI(Btn_Github.Tag.ToString() + "/DrawShare/issues/new");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ApplicationMode == Modes.Client && client != null)
                client.Stop();
            else if (ApplicationMode == Modes.Server && server != null)
                server.Stop();

            CleanUp();
            /*
            if (MessageBox.Show(this, Translation.ExitDlg_Text, Translation.ExitDlg_Caption, MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            */
        }

        private void MI_Debug_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            ThreadPool.QueueUserWorkItem(delegate
            {
                Thread.Sleep(1000 * 2);
                UseDispatcher(SP_Board, delegate
                {
                    DrawingLocked = true;
                    SP_Board.IsEnabled = false;
                });
            });
        }
    }
    #endregion GUI-Methodes
}
