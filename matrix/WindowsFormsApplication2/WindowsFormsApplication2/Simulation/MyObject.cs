using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StrategyTester
{
    class MyObject
    {
        public Point Coords;
        public Size Size;

        public MyObject(Point coords, Size size)
        {
            Coords = coords;
            Size = size;
        }

        public bool IsPointIn(Point point)
        {
            return point.X > Coords.X && point.X < Coords.X + Size.Width &&
                   point.Y > Coords.Y && point.Y < Coords.Y + Size.Height;
        }

        public bool IsPointIn(MyPoint point)
        {
            return point.X > Coords.X && point.X < Coords.X + Size.Width &&
                   point.Y > Coords.Y && point.Y < Coords.Y + Size.Height;
        }
    }
}
