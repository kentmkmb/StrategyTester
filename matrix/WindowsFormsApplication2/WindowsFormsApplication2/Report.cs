using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    public class Report
    {
        public Point Coords;
        private double angleInRadians;
        public double AngleInRadians
        {
            get { return angleInRadians; }
            set
            {
                while (value < 0)
                {
                    value += 2 * Math.PI;
                }
                while (value >= 2*Math.PI)
                {
                    value -= 2 * Math.PI;
                }
                angleInRadians = value;
            }
        }
        public bool Success;

        public Report(double angle, Point coords, bool success)
        {
            AngleInRadians = angle;
            Coords = coords;
            Success = success;
        }
    }
}
