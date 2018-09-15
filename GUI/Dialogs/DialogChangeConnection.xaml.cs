using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }

        public DialogChangeConnection(string title, string ipLabel, string ip, string portLabel, int port, string cancelText, string connectText) : this()
        {
            Title = title;
            GroupBox_IP.Header = ipLabel;
            TB_IPAddress.Text = ip;
            GroupBox_Port.Header = portLabel;
            TB_Port.Text = port.ToString();
            Btn_Cancel.Content = cancelText;
            Btn_Connect.Content = connectText;
        }


        public string IPAddress { get; private set; }
        public int Port { get; private set; }


        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            Regex RegexIP = new Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.)" +
                "{3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            bool error = false;
            string errorMsg = "";
            string inputIP = TB_IPAddress.Text.ToLower();
            if (inputIP == "localhost" || RegexIP.Match(inputIP).Success)
            {
                IPAddress = inputIP;
            }
            else
            {
                error = true;
                errorMsg += "Ungültige IP-Adresse";
            }
            int inputPort;
            if (int.TryParse(TB_Port.Text, out inputPort))
            {
                if (inputPort >= 65535)
                {
                    error = true;
                    errorMsg += "\nUngültiger Port";
                }
                else
                {
                    Port = inputPort;
                }
            }
            else
            {
                error = true;
                errorMsg += "\nUngültiger Port";
            }
            if (error)
            {
                MessageBox.Show(errorMsg, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
