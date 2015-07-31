using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyTester
{
    public struct MyPoint
    {
        public double X;
        public double Y;

        public MyPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public MyPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }
    }
}
