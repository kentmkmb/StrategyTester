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
        public List<MyObject> Objects;
        public Robot OurRobot;
        const double angleDelta = Math.PI / 50;
        const int coordsDelta = 4;
        public bool Moving { get; private set; }

        public World(MyPoint robotCoords, double robotAngle)
        {
            Moving = false;
            Objects = new List<MyObject>();
            OurRobot = new Robot(robotCoords, robotAngle);
        }

        bool Forward(Report originalState, Forward command)
        {
            Moving = true;
            var isMoveSuccessfull = true;
            var timeDelta = command.Time / 100;
            var targetCoords = new MyPoint(OurRobot.Coords.X + command.Time * command.Speed * Math.Cos(OurRobot.Angle),
                                           OurRobot.Coords.Y + command.Time * command.Speed * Math.Sin(OurRobot.Angle));
            for (var i = 0.0; i < command.Time - timeDelta; i+=timeDelta)
            {
                OurRobot.Coords.X += timeDelta * command.Speed * Math.Cos(OurRobot.Angle);
                OurRobot.Coords.Y += timeDelta * command.Speed * Math.Sin(OurRobot.Angle);
                if (Objects.Any(x => x.IsPointIn(OurRobot.Coords)))
                {
                    OurRobot.Angle = originalState.AngleInRadians;
                    OurRobot.Coords = originalState.Coords;
                    isMoveSuccessfull = false;
                    break;
                }
                Thread.Sleep((int)(timeDelta * 1000));
            }
            if (isMoveSuccessfull)
                OurRobot.Coords = targetCoords;
            Moving = false;
            return isMoveSuccessfull;
        }

        bool Rotate(Report originalState, Rotate command)
        {
            Moving = true;
            bool isMoveSuccessfull = true;
            var targetAngle = OurRobot.Angle + command.AngleSpeed * command.Time;
            var timeDelta = command.Time / 100;
            for (var i = 0.0; i < command.Time - timeDelta; i += timeDelta)
            {
                OurRobot.Angle += timeDelta * command.AngleSpeed;
                Thread.Sleep((int)(timeDelta * 1000));
            }
            OurRobot.Angle = targetAngle;
            Moving = false;
            return isMoveSuccessfull;
        }

        bool Nothing(Report originalState, Nothing command)
        {
            return true;
        }

        public Report MakeMoves(Report originalState, List<LowLevelCommand> movesList)
        {
            bool isSuccess = true;
            foreach (var move in movesList)
            {
                var type = move.GetType().Name;
                isSuccess = (bool)this
                    .GetType()
                    .GetMethod(type, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(this, new object[] { originalState, move });
                if (!isSuccess)
                {
                    OurRobot.Angle = originalState.AngleInRadians;
                    OurRobot.Coords = originalState.Coords;
                    break;
                }
            }
            var resultingState = new Report(OurRobot.Angle, OurRobot.Coords, isSuccess);
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
