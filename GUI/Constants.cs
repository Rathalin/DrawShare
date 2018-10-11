using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public static class Constants
    {
        private static string _regexLocalhost = "localhost";
        private static string _regexIP = "(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])";
        private static string _regexPort = "([0-9]){1,5}";

        public static string RegexIP { get; private set; } = "^(" + _regexIP + ")|" + _regexLocalhost + "$";
        public static string RegexIPPort { get; private set; } = "^(" + _regexIP + ")|" + _regexLocalhost + ":" + _regexPort + "$";
    }
}
