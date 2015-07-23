using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    public interface ITranslator
    {
        List<LowLevelCommand> Translate(Report current, MovementTo action);
        List<LowLevelCommand> Translate(Report current, StoppingAt action);
    }
}
