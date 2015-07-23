using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EurobotStrategyCreator
{
    class MyObject
    {
        public Point Coords;
        public double Height;
        public double Width;

        public MyObject(Point coords, double height, double width)
        {
            Coords = coords;
            Height = height;
            Width = width;
        }
    }
}
