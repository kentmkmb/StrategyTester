using System.Collections.Generic;

namespace StrategyTester
{
    public abstract class State
    {
        public State Previous;
        public State Next;
        public Strategy Alternative;

        public abstract List<LowLevelCommand> GetTranslation(ITranslator translator, Report current);

        public abstract override string ToString();
    }
}