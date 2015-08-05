using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using StrategyBuilder;

namespace StrategyVisualizer
{
    class Polygon
    {
        public Point Coords;
        public Size Size;

        public Polygon(Point coords, Size size)
        {
            Coords = coords;
            Size = size;
        }

        public bool IsPointIn(Point point)
        {
            return point.X > Coords.X && point.X < Coords.X + Size.Width &&
                   point.Y > Coords.Y && point.Y < Coords.Y + Size.Height;
        }

        public bool IsPointIn(PointD point)
        {
            return point.X > Coords.X && point.X < Coords.X + Size.Width &&
                   point.Y > Coords.Y && point.Y < Coords.Y + Size.Height;
        }
    }
}
