using System.Collections.Generic;

namespace StrategyBuilder
{
    public class StoppingAt : State
    {
        public PointD Coords;
        public double AngleInRadians;

        public StoppingAt(double angle, PointD coords)
        {
            AngleInRadians = angle;
            Coords = coords;
            Next = null;
            Previous = null;
            Alternative = null;
        }
        public override string ToString()
        {
            return string.Format("StopAt({0}, {1}, {2})", Coords.X, Coords.Y, AngleInRadians);
        }
        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            return translator.Translate(current, this);
        }
    }
}
