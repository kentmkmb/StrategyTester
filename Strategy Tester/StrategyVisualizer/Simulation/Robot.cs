using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StrategyBuilder;
using StrategyBuilder.Translation.StrategyTester;

namespace StrategyVisualizer
{
    class Robot
    {
        public PointD Coords;
        public double Angle;
        public double Size;

        public Robot(PointD coords, double angle)
        {
            Coords = coords;
            Angle = angle;
        }

        public bool Forward(StrategyTesterReport originalState, Forward command, List<Polygon> obstacles, bool doFast)
        {
            var isMoveSuccessfull = true;
            var timeDelta = command.Time / 100;
            var targetCoords = new PointD(Coords.X + command.Time * command.Speed * Math.Cos(Angle),
                                          Coords.Y + command.Time * command.Speed * -Math.Sin(Angle));
            for (var i = 0.0; i < command.Time - timeDelta; i += timeDelta)
            {
                Coords.X += timeDelta * command.Speed * Math.Cos(Angle);
                Coords.Y += timeDelta * command.Speed * -Math.Sin(Angle);
                if (obstacles.Any(x => x.IsPointIn(Coords)))
                {
                    Angle = originalState.AngleInRadians;
                    Coords = originalState.Coords;
                    isMoveSuccessfull = false;
                    break;
                }
                if (!doFast) Thread.Sleep((int)(timeDelta * 1000));
            }
            if (isMoveSuccessfull) Coords = targetCoords;
            return isMoveSuccessfull;
        }

        public bool Nothing(StrategyTesterReport originalState, Nothing command, List<Polygon> obstacles, bool doFast)
        {
            return true;
        }

        public bool Rotate(StrategyTesterReport originalState, Rotate command, List<Polygon> obstacles, bool doFast)
        {
            bool isMoveSuccessfull = true;
            var targetAngle = Angle + command.AngleSpeed * command.Time;
            var timeDelta = command.Time / 100;
            for (var i = 0.0; i < command.Time - timeDelta; i += timeDelta)
            {
                Angle += timeDelta * command.AngleSpeed;
                if (obstacles.Any(x => x.IsPointIn(Coords)))
                {
                    Angle = originalState.AngleInRadians;
                    Coords = originalState.Coords;
                    isMoveSuccessfull = false;
                    break;
                }
                if (!doFast) Thread.Sleep((int)(timeDelta * 1000));
            }
            Angle = targetAngle;
            return isMoveSuccessfull;
        }
    }
}
