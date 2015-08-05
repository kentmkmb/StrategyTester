using System;

namespace StrategyBuilder
{
    public static class AngleCaculator
    {
        class Vector
        {
            public double X, Y, Length;
            public Vector(PointD begin, PointD end)
            {
                if (begin.Equals(end)) throw new ArgumentException();
                X = end.X - begin.X;
                Y = end.Y - begin.Y;
                Length = Math.Sqrt(X * X + Y * Y);
            }
        }
        public static double CalculateAngle(PointD begin, PointD end)
        {
            var horizontal = new Vector(new PointD(0, 0), new PointD(1, 0));
            var current = new Vector(begin, end);
            if (current.X == 0)
                return current.Y > 0 ? Math.PI/2 : 3*Math.PI/2;
            if (current.Y == 0)
                return current.X > 0 ? 0 : Math.PI;
            var scalarMult = (horizontal.X * current.X) + (horizontal.Y * current.Y);
            var angleCos = scalarMult / (horizontal.Length * current.Length);
            var angle = Math.Acos(angleCos);
            return current.Y > 0 ? angle : 2*Math.PI - angle;
        }
    }
}
