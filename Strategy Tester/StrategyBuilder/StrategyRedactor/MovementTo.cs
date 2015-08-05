using System;
using System.Collections.Generic;
using System.Drawing;

namespace StrategyBuilder
{
    public class MovementTo : State
    {
        public PointD Coords;
        public MovementTo(PointD coords)
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
        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            return translator.Translate(current, this);
        }
    }
}
