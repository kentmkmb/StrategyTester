using System;
using System.Drawing;

namespace StrategyTester
{
    public static class AngleCaculator
    {
        class Vector
        {
            public double X, Y, Length;
            public Vector(MyPoint begin, MyPoint end)
            {
                if (begin.Equals(end)) throw new ArgumentException();
                X = end.X - begin.X;
                Y = end.Y - begin.Y;
                Length = Math.Sqrt(X * X + Y * Y);
            }
        }
        public static double CalculateAngle(MyPoint begin, MyPoint end)
        {
            var horizontal = new Vector(new MyPoint(0, 0), new MyPoint(1, 0));
            var current = new Vector(begin, end);
            if (current.X == 0)
            {
                if (current.Y > 0) return Math.PI / 2;
                else return 3 * Math.PI / 2;
            }
            if (current.Y == 0)
            {
                if (current.X > 0) return 0;
                else return Math.PI;
            }
            double scalarMult = (horizontal.X * current.X) + (horizontal.Y * current.Y);
            double angleCos = scalarMult / (horizontal.Length * current.Length);
            double angle = Math.Acos(angleCos);
            if (current.Y > 0) return angle;
            else return 2 * Math.PI - angle;
        }
    }
}
