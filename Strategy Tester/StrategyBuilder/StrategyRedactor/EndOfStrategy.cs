using System.Collections.Generic;
using StrategyBuilder.Translation;

namespace StrategyBuilder
{
    public class EndOfStrategy : State
    {
        public EndOfStrategy()
        {
            Next = null;
            Previous = null;
            Alternative = null;
        }
        public override string ToString()
        {
            return "End";
        }
        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            return translator.Translate(current, this);
        }
    }
}
