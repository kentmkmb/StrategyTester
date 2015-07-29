using System;
using System.Collections.Generic;

namespace StrategyTester
{
    public interface IStrategy
    {
        Tuple<List<LowLevelCommand>, State> GetNextState(Report current);
        void GoToPreviousState(int number);
    }
}
