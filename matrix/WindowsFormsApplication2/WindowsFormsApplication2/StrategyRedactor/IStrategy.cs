using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EurobotStrategyCreator
{
    public interface IStrategy
    {
        Tuple<List<LowLevelCommand>, State> GetNextState(Report current);
        void GoToPreviousState(int number);
    }
}
