using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication.MessageTypes
{
    public class UserCount : Message
    {
        public UserCount()
        {
            Count = 0;
        }

        public UserCount(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}
