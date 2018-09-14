using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GUI
{
    public class CustomBrush
    {
        public CustomBrush() { }

        public CustomBrush(SolidColorBrush colorBrush, double thickness)
        {
            ColorBrush = colorBrush;
            Thickness = thickness;
        }

        public CustomBrush(string colorARGB, double thickness)
        {
            ColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorARGB));
            Thickness = thickness;
        }

        public SolidColorBrush ColorBrush { get; set; }
        public double Thickness { get; set; }
    }
}
