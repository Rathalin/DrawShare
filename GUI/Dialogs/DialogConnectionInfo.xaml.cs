using GUI.Visualisation;
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
    /// Interaktionslogik für DialogConnectionInfo.xaml
    /// </summary>
    public partial class DialogConnectionInfo : Window
    {
        public DialogConnectionInfo()
        {
            InitializeComponent();
            Icon = ImageResource.DrawShareLogo1;
            Btn_CopyLocal.Background = ImageResource.Copy;
            Btn_CopyGlobal.Background = ImageResource.Copy;
        }

        public DialogConnectionInfo(string title, string infoText, string ipLabel, string ipaddressLocal, string ipaddressGlobal, string portLabel, int port, string closeText) : this()
        {
            Title = title;
            TBl_Info.Text = infoText;
            GroupBox_IP_Local.Header = ipLabel + " (local)";
            GroupBox_IP_Global.Header = ipLabel + " (global)";
            TBl_IPAddress_Local.Text = ipaddressLocal;
            TBl_IPAddress_Global.Text = ipaddressGlobal;
            GroupBox_Port.Header = portLabel;
            TBl_Port.Text = port.ToString();
            Btn_Close.Content = closeText;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string ip = "";
            if (btn.Tag.ToString() == "Global")
                ip = TBl_IPAddress_Global.Text;
            else if (btn.Tag.ToString() == "Local")
                ip = TBl_IPAddress_Local.Text;
            Regex RegexIPPort = new Regex(Constants.RegexIPPort);
            if (RegexIPPort.Match(ip + ":" + TBl_Port).Success)
            {
                Clipboard.SetText(ip + ":" + TBl_Port.Text);
            }
        }
    }
}
