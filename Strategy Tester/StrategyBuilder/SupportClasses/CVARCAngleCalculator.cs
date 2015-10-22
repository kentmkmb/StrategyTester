using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StrategyBuilder.SupportClasses
{
    public class CVARCAngleCalculator
    {
        public static double Distance(PointD one, PointD two)
        {
            var a = Math.Abs(one.X - two.X);
            var b = Math.Abs(one.Y - two.Y);
            return Math.Sqrt(a * a + b * b);
        }

        public static double GetAngle(PointD one, PointD two, double angle)
        {
            angle = angle % 360;
            angle = angle * Math.PI / 180;
            var vectorOfTargets = two - one;
            var currentVector = new PointD(Math.Cos(angle), Math.Sin(angle));
            var vectorProduct = currentVector.X * vectorOfTargets.Y - currentVector.Y * vectorOfTargets.X;
            var scalarProduct = currentVector.X * vectorOfTargets.X + currentVector.Y * vectorOfTargets.Y;
            var newAngle = Math.Acos(scalarProduct / (Distance(new PointD(0, 0), currentVector) * Distance(new PointD(0, 0), vectorOfTargets)));
            vectorProduct = vectorProduct > 0 ? 1 : -1;
            return newAngle * vectorProduct;
        }
    }
}
