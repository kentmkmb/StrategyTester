using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using StrategyBuilder;

namespace StrategyVisualizer
{
    class World
    {
        const double angleDelta = Math.PI / 50;
        const int coordsDelta = 4;
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
                    .GetMethod(type, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
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
            var task = new Task<Report>(() => { return MakeMoves(currentState, movesList); });
            task.Start();
            return task;
        }
    }
}
