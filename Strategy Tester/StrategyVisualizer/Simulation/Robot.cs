using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StrategyBuilder;

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

        public bool Forward(Report originalState, Forward command, List<Polygon> obstacles)
        {
            var isMoveSuccessfull = true;
            var timeDelta = command.Time / 100;
            var targetCoords = new PointD(Coords.X + command.Time * command.Speed * Math.Cos(Angle),
                                          Coords.Y + command.Time * command.Speed * Math.Sin(Angle));
            for (var i = 0.0; i < command.Time - timeDelta; i += timeDelta)
            {
                Coords.X += timeDelta * command.Speed * Math.Cos(Angle);
                Coords.Y += timeDelta * command.Speed * Math.Sin(Angle);
                if (obstacles.Any(x => x.IsPointIn(Coords)))
                {
                    Angle = originalState.AngleInRadians;
                    Coords = originalState.Coords;
                    isMoveSuccessfull = false;
                    break;
                }
                Thread.Sleep((int)(timeDelta * 1000));
            }
            if (isMoveSuccessfull) Coords = targetCoords;
            return isMoveSuccessfull;
        }

        public bool Nothing(Report originalState, Nothing command, List<Polygon> obstacles)
        {
            return true;
        }

        public bool Rotate(Report originalState, Rotate command, List<Polygon> obstacles)
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
                Thread.Sleep((int)(timeDelta * 1000));
            }
            Angle = targetAngle;
            return isMoveSuccessfull;
        }
    }
}
