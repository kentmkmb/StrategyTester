using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    class OldStrategy : IStrategy
    {
        int pointer;
        List<State> generalPlan;
        List<List<State>> alternativePlans;

        public OldStrategy()
        {
            pointer = 0;
            generalPlan = new List<State>{
                MoveTrougth(200, 150),
                MoveTrougth(150, 200),
                MoveTo(90, 60, 90)
            };
            alternativePlans = new List<List<State>>();
        }

        public State GetNextState(Report current)
        {
            if (pointer >= generalPlan.Count) return new MovementTo(current.Coords);
            return generalPlan[pointer++];
        }

        # region AvalibleActions
        private StoppingAt MoveTo(int x, int y, double angle)
        {
            return new StoppingAt(angle, new Point(x, y));
        }
        private MovementTo MoveTrougth(int x, int y)
        {
            return new MovementTo(new Point(x, y));
        }
        #endregion
    }
}
