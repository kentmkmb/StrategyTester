using System.Collections.Generic;

namespace StrategyTester
{
    public interface ITranslator
    {
        List<LowLevelCommand> Translate(Report current, MovementTo action);
        List<LowLevelCommand> Translate(Report current, StoppingAt action);
        List<LowLevelCommand> Translate(Report current, EndOfStrategy action);
    }
}
