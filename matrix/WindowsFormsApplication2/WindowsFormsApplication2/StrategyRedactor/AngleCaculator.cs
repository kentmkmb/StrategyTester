using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator.StrategyRedactor
{
    public abstract class AngleCaculator
    {
        class Vector
        {
            public double X, Y, Length;
            public Vector(Point begin, Point end)
            {
                if (begin.Equals(end)) throw new ArgumentException();
                X = end.X - begin.X;
                Y = end.Y - begin.Y;
                Length = Math.Sqrt(X * X + Y * Y);
            }
        }
        public double CalculateAngle(Point begin, Point end)
        {
            var vertical = new Vector(new Point(0, 0), new Point(0, 1));
            var current = new Vector(begin, end);
            if (current.X == 0)
            {
                if (current.Y > 0) return 0;
                else return Math.PI;
            }
            if (current.Y == 0)
            {
                if (current.X > 0) return Math.PI / 2;
                else return 3 * Math.PI / 2;
            }
            double scalarMult = (vertical.X * current.X) + (vertical.Y * current.Y);
            double angleCos = scalarMult / (vertical.Length * current.Length);
            double angle = Math.Acos(angleCos);
            if (current.X > 0) return angle;
            else return 2 * Math.PI - angle;
        }
    }
}
