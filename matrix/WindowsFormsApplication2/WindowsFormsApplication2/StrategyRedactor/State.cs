using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EurobotStrategyCreator
{
    public abstract class State
    {
        public State Previous;
        public State Next;
        public Strategy Alternative;

        public abstract override string ToString();
    }
}