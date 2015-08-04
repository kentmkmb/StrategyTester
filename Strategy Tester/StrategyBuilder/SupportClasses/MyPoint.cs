using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder
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

        public override string ToString()
        {
            return String.Format("Point({0}, {1})", X, Y);
        }
    }
}
