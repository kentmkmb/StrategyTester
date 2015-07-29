using System.Collections.Generic;

namespace StrategyTester
{
    public class EndOfStrategy : State
    {
        public EndOfStrategy()
        {
            this.Next = null;
            this.Previous = null;
            this.Alternative = null;
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
