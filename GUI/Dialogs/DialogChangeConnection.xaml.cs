using GUI.Visualisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogChangeConnection.xaml
    /// </summary>
    public partial class DialogChangeConnection : Window
    {
        public DialogChangeConnection()
        {
            InitializeComponent();
            Icon = ImageResource.DrawShareLogo;
        }

        public DialogChangeConnection(string title, string ipLabel, string ip, string portLabel, int port, string cancelText, string connectText,
            string pasteText, string invalidIP, string invalidPort, string invalidInput) : this()
        {
            Title = title;
            Btn_Paste.Background = ImageResource.Paste;
            GroupBox_IP.Header = ipLabel;
            TB_IPAddress.Text = ip;
            GroupBox_Port.Header = portLabel;
            TB_Port.Text = port.ToString();
            Btn_Cancel.Content = cancelText;
            Btn_Connect.Content = connectText;
            Btn_Paste.ToolTip = pasteText;
            this.invalidIP = invalidIP;
            this.invalidPort = invalidPort;
            this.invalidInput = invalidInput;
        }


        public string IPAddress { get; private set; }
        public int Port { get; private set; }

        private string invalidIP;
        private string invalidPort;
        private string invalidInput;


        public void Paste()
        {
            Regex RegexIPPort = new Regex(Constants.RegexIPPort);
            string paste = Clipboard.GetText();
            if (RegexIPPort.Match(paste).Success)
            {
                var arr = paste.Split(new char[] { ':' });
                IPAddress = arr[0];
                Port = int.Parse(arr[1]);
                TB_IPAddress.Text = IPAddress;
                TB_Port.Text = Port.ToString();
                Blink(Brushes.LightGreen, 1);
            }
            else
            {
                Blink(Brushes.LightCoral, 2);
            }
        }

        public void UseDispatcher(Control el, Action func)
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


        private void Btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            Regex RegexIP = new Regex(Constants.RegexIP);
            bool error = false;
            string errorMsg = "";
            string inputIP = TB_IPAddress.Text.ToLower();
            if (RegexIP.Match(inputIP).Success)
            {
                IPAddress = inputIP;
            }
            else
            {
                error = true;
                errorMsg += invalidIP;
            }
            int inputPort;
            if (int.TryParse(TB_Port.Text, out inputPort))
            {
                if (inputPort >= 65535)
                {
                    error = true;
                    errorMsg += "\n" + invalidPort;
                }
                else
                {
                    Port = inputPort;
                }
            }
            else
            {
                error = true;
                errorMsg += "\n" + invalidPort;
            }
            if (error)
            {
                MessageBox.Show(errorMsg, invalidInput, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void Blink(Brush brush, int cnt)
        {

            ThreadPool.QueueUserWorkItem(delegate
            {
                for (int i = 0; i < cnt; i++)
                {
                    UseDispatcher(this, delegate
                    {
                        TB_IPAddress.Background = brush;
                        TB_Port.Background = brush;
                    });
                    Thread.Sleep(100);
                    UseDispatcher(this, delegate
                    {
                        TB_IPAddress.Background = Brushes.White;
                        TB_Port.Background = Brushes.White;
                    });
                    if (i + 1 < cnt)
                        Thread.Sleep(100);
                }
            });
        }

        private void Btn_Paste_Click(object sender, RoutedEventArgs e)
        {
            Paste();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
