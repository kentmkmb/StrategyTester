using System.Collections.Generic;
using StrategyBuilder.Translation;

namespace StrategyBuilder
{
    public interface ITranslator
    {
        List<LowLevelCommand> Translate(Report current, MovementTo action);
        List<LowLevelCommand> Translate(Report current, StoppingAt action);
        List<LowLevelCommand> Translate(Report current, EndOfStrategy action);
    }
}
