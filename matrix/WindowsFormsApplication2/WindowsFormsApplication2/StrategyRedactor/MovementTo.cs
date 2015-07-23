using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    public class MovementTo : State
    {
        public Point Coords;
        public MovementTo(Point coords)
        {
            Coords = coords;
            this.Next = null;
            this.Previous = null;
            this.Alternative = null;
        }
        public override string ToString()
        {
            return String.Format("MoveTo({0}, {1})", Coords.X, Coords.Y);
        }
    }
}
