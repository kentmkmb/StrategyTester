using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    public class StoppingAt : State
    {
        public Point Coords;
        public double AngleInRadians;

        public StoppingAt(double angle, Point coords)
        {
            AngleInRadians = angle;
            Coords = coords;
            this.Next = null;
            this.Previous = null;
            this.Alternative = null;
        }
        public override string ToString()
        {
            return String.Format("StopAt({0}, {1}, {2})", Coords.X, Coords.Y, AngleInRadians);
        }
    }
}
