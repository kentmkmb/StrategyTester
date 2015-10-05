using System.Collections.Generic;
using StrategyBuilder.Translation;

namespace StrategyBuilder
{
    public class MovementTo : State
    {
        public PointD Coords;
        public MovementTo(PointD coords)
        {
            Coords = coords;
            Next = null;
            Previous = null;
            Alternative = null;
        }
        public override string ToString()
        {
            return string.Format("MoveTo({0}, {1})", Coords.X, Coords.Y);
        }
        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            return translator.Translate(current, this);
        }
    }
}
