using System;

namespace StrategyBuilder
{
    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("Point({0}, {1})", X, Y);
        }

        public static PointD operator -(PointD objTwo, object objOne)
        {
            var pointOne = (PointD)objOne;
            return new PointD(objTwo.X - pointOne.X, objTwo.Y - pointOne.Y);
        }

        public override bool Equals(object obj)
        {
            var p = (PointD)obj;
            return Math.Abs(p.X - X) < 0.0001 && Math.Abs(Y - p.Y) < 0.0001;
        }
    }
}
