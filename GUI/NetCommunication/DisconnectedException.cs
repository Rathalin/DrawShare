using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication
{
    public class DisconnectedException : Exception
    {
        public DisconnectedException() { }

        public DisconnectedException(string message) : base(message) { }

        public DisconnectedException(string message, Exception inner) : base(message, inner) { }
    }
}
