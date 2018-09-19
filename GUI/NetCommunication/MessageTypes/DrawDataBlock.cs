using GUI.NetCommunication.MessageTypes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.NetCommunication.MessageTypes
{
    public class DrawDataBlock : Message
    {
        public DrawDataBlock()
        {
            Lines = new List<Line>();
        }

        public DrawDataBlock(List<Line> lineList)
        {
            Lines = lineList;
        }

        public DrawDataBlock(string color, double thickness, List<Line> lineList) : this (lineList)
        {
            Color = color;
            Thickness = thickness;
        }


        public double Thickness { get; set; }
        public string Color { get; set; }
        public List<Line> Lines { get; set; }
    }
}
