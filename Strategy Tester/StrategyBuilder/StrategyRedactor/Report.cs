using System;
using RoboMovies;

namespace StrategyBuilder
{
    public abstract class Report
    {
        public bool Success;
    }

    public class StrategyTesterReport : Report
    {
        public PointD Coords;
        private double angleInRadians;
        public double AngleInRadians
        {
            get { return angleInRadians; }
            set
            {
                while (value < 0)
                {
                    value += 2 * Math.PI;
                }
                while (value >= 2 * Math.PI)
                {
                    value -= 2 * Math.PI;
                }
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
        public RMClient<RMClient<CommonSensorData>> Client;

        public CVARCReport(RMClient<RMClient<CommonSensorData>> client, bool success)
        {
            Client = client;
            Success = success;
        }
    }
}
