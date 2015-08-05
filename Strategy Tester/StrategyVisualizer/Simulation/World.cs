using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using StrategyBuilder;

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

        public Report MakeMoves(Report originalState, List<LowLevelCommand> movesList)
        {
            Moving = true;
            bool isSuccess = true;
            foreach (var move in movesList)
            {
                var type = move.GetType().Name;
                isSuccess = (bool)OurRobot
                    .GetType()
                    .GetMethod(type, BindingFlags.Instance | BindingFlags.Public)
                    .Invoke(OurRobot, new object[] { originalState, move, Objects });
                if (!isSuccess)
                {
                    OurRobot.Angle = originalState.AngleInRadians;
                    OurRobot.Coords = originalState.Coords;
                    break;
                }
            }
            var resultingState = new Report(OurRobot.Angle, OurRobot.Coords, isSuccess);
            Moving = false;
            return resultingState;
        }

        public void SetState(Report state)
        {
            OurRobot.Angle = state.AngleInRadians;
            OurRobot.Coords = state.Coords;
        }

        public Task<Report> TryMove(Report currentState, List<LowLevelCommand> movesList)
        {
            var task = new Task<Report>(() => MakeMoves(currentState, movesList));
            task.Start();
            return task;
        }
    }
}
