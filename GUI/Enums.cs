using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public static class Enums
    {
        public enum Modes { Undefined, Server, Client }
        public enum LogLevels { NoLog, Error, Warning, Info, Debug }
        public enum ConnectionStatus { Disconnected, ClientConnecting, ClientConnected, ServerConnecting, ServerOnline}
    }
}
