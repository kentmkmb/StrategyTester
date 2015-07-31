using System;
using System.Collections.Generic;
using System.Drawing;

namespace StrategyTester
{
    public class StoppingAt : State
    {
        public MyPoint Coords;
        public double AngleInRadians;

        public StoppingAt(double angle, MyPoint coords)
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
        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            return translator.Translate(current, this);
        }
    }
}
