using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication.MessageTypes
{
    public class DrawData : Message
    {
        public DrawData() { }
        
        public DrawData(double x1, double y1, double x2, double y2, double thickness, string color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Thickness = thickness;
            Color = color;
        }

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Thickness { get; set; }
        public string Color { get; set; }
    }
}
