using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
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
    }
}
