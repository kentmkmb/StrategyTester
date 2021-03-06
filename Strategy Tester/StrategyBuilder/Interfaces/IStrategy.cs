﻿using System;
using System.Collections.Generic;
using StrategyBuilder.Translation;

namespace StrategyBuilder
{
    public interface IStrategy
    {
        Tuple<List<LowLevelCommand>, State> GetNextState(Report current);
        void GoToPreviousState(int number);
        State First { get; set; }
    }
}
