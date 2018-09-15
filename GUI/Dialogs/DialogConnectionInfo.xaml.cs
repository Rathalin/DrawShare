using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        public DialogConnectionInfo(string title, string infoText, string ipLabel, string ipaddress, string portLabel, int port, string closeText) : this()
        {
            Title = title;
            TBl_Info.Text = infoText;
            GroupBox_IP.Header = ipLabel;
            TBl_IPAddress.Text = ipaddress;
            GroupBox_Port.Header = portLabel;
            TBl_Port.Text = port.ToString();
            Btn_Close.Content = closeText;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
