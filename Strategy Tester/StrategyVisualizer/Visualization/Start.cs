using System;
using System.Collections.Generic;
using StrategyBuilder;

namespace StrategyVisualizer
{
    public class Start : State
    {
        public PointD Coords;

        public Start(PointD coords)
        {
            Coords = coords;
        }

        public override string ToString()
        {
            return Coords.ToString();
        }

        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            throw new NotImplementedException();
        }
    }
}
