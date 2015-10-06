using System;
using RoboMovies;

namespace StrategyBuilder
{
    public abstract class Report
    {
        public bool Success;
        protected double angleInRadians;
    }

    public class StrategyTesterReport : Report
    {
        public PointD Coords;
        public double AngleInRadians
        {
            get { return angleInRadians; }
            set
            {
                while (value < 0)
                    value += 2 * Math.PI;
                while (value >= 2 * Math.PI)
                    value -= 2 * Math.PI;
                angleInRadians = value;
            }
        }

        public StrategyTesterReport(double angle, PointD coords, bool success)
        {
            AngleInRadians = angle;
            Coords = coords;
            Success = success;
        }
    }

    public class CVARCReport : Report
    {
        public PointD Coords;
        public Level2Client Client;
        public double AngleInRadians
        {
            get { return angleInRadians; }
            set
            {
                while (value < 0)
                    value += 2 * Math.PI;
                while (value >= 2 * Math.PI)
                    value -= 2 * Math.PI;
                angleInRadians = value;
            }
        }

        public CVARCReport(FullMapSensorData sensorsData, Level2Client client, bool success)
        {
            AngleInRadians = (sensorsData.SelfLocation.Angle / 180) * Math.PI;
            Client = client;
            Coords = new PointD(sensorsData.SelfLocation.X, sensorsData.SelfLocation.Y);
            Success = success;
        }
    }
}
