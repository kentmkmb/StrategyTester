using System.Collections.Generic;
using System.Reflection;
using StrategyBuilder;
using StrategyBuilder.Translation;

namespace StrategyVisualizer
{
    class World
    {
        public List<Polygon> Objects { get; private set; }
        public Robot OurRobot { get; private set; }
        public bool Moving { get; private set; }

        public World(PointD robotCoords, double robotAngle)
        {
            Moving = false;
            Objects = new List<Polygon>();
            OurRobot = new Robot(robotCoords, robotAngle);
        }

        public StrategyTesterReport TryMove(StrategyTesterReport originalState, List<LowLevelCommand> movesList, bool doFast)
        {
            Moving = true;
            var isSuccess = true;
            foreach (var move in movesList)
            {
                var type = move.GetType().Name;
                isSuccess = (bool)OurRobot
                    .GetType()
                    .GetMethod(type, BindingFlags.Instance | BindingFlags.Public)
                    .Invoke(OurRobot, new object[] { originalState, move, Objects, doFast });
                if (isSuccess) continue;
                OurRobot.Angle = originalState.AngleInRadians;
                OurRobot.Coords = originalState.Coords;
                break;
            }
            var resultingState = new StrategyTesterReport(OurRobot.Angle, OurRobot.Coords, isSuccess);
            Moving = false;
            return resultingState;
        }

        public void SetState(StrategyTesterReport state)
        {
            OurRobot.Angle = state.AngleInRadians;
            OurRobot.Coords = state.Coords;
        }
    }
}
