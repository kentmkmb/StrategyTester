using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyTester
{
    class World
    {
        public List<MyObject> Objects;
        public Robot OurRobot;
        public Robot Opponent;
        public bool Moving { get; private set; }

        public World(Point robotCoords, double robotAngle)
        {
            Moving = false;
            Objects = new List<MyObject>();
            OurRobot = new Robot(robotCoords, robotAngle);
        }

        private Report Move(Report currentState, Report desiredState)
        {
            const double angleDelta = Math.PI / 50;
            const int coordsDelta = 4;
            Moving = true;
            while (Math.Abs(OurRobot.Angle - desiredState.AngleInRadians) > angleDelta)
            {
                if (OurRobot.Angle < desiredState.AngleInRadians) OurRobot.Angle += angleDelta;
                else OurRobot.Angle -= angleDelta;
                Thread.Sleep(25);
            }
            OurRobot.Angle = desiredState.AngleInRadians;
            while (Math.Abs(OurRobot.Coords.X - desiredState.Coords.X) > Math.Abs(coordsDelta * Math.Cos(OurRobot.Angle)) ||
                  Math.Abs(OurRobot.Coords.Y - desiredState.Coords.Y) > Math.Abs(coordsDelta * Math.Sin(OurRobot.Angle)))
            {
                if (OurRobot.Coords.X < desiredState.Coords.X) OurRobot.Coords.X += (int)Math.Floor(Math.Abs(coordsDelta * Math.Cos(OurRobot.Angle)));
                else OurRobot.Coords.X -= (int)Math.Floor(Math.Abs(coordsDelta * Math.Cos(OurRobot.Angle)));
                if (OurRobot.Coords.Y < desiredState.Coords.Y) OurRobot.Coords.Y += (int)Math.Floor(Math.Abs(coordsDelta * Math.Sin(OurRobot.Angle)));
                else OurRobot.Coords.Y -= (int)Math.Floor(Math.Abs(coordsDelta * Math.Sin(OurRobot.Angle)));
                Thread.Sleep(25);
            }
            OurRobot.Coords = desiredState.Coords;
            Moving = false;
            return new Report(OurRobot.Angle, OurRobot.Coords, true);
        }

        public Task<Report> TryMove(Report currentState, Report desiredState)
        {
            var task = new Task<Report>(() => { return Move(currentState, desiredState); });
            task.Start();
            return task;
        }
    }
}
