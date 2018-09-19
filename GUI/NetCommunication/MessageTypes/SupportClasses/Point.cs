using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication.MessageTypes.SupportClasses
{
    public class Point<T>
    {
        public Point() { }

        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }


        public T X { get; set; }
        public T Y { get; set; }
    }
}
