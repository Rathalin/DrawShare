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

        public DialogConnectionInfo(string infotext, string ipaddress, int port) : this()
        {
            TBl_Info.Text = infotext;
            TBl_IPAddress.Text = ipaddress;
            TBl_Port.Text = port.ToString();
        }

        public DialogConnectionInfo(string title, string infotext, string ipaddress, int port) : this(infotext, ipaddress, port)
        {
            Title = title;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
