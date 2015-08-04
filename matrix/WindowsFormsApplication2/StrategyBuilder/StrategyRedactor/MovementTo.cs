using System;
using System.Collections.Generic;
using System.Drawing;

namespace StrategyTester
{
    public class MovementTo : State
    {
        public MyPoint Coords;
        public MovementTo(MyPoint coords)
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
