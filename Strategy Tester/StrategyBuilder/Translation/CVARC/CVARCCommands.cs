using System;
using RoboMovies;

namespace StrategyBuilder.Translation.CVARC
{
    public abstract class CVARCLowLevelCommand : LowLevelCommand
    {
        public Func<FullMapSensorData> Action;
    }

    public class Forward : CVARCLowLevelCommand
    {
        private readonly double distance;

        public Forward(double distance, Level2Client client)
        {
            this.distance = distance;
            Action = () => client.Move(distance);
        }

        public override string ToString()
        {
            return string.Format("Move({0})", distance);
        }
    }

    public class Rotate : CVARCLowLevelCommand
    {
        private readonly double angle;

        public Rotate(double angle, Level2Client client)
        {
            this.angle = angle;
            Action = () => client.Rotate(angle);
        }

        public override string ToString()
        {
            return string.Format("Rotate({0})", angle);
        }
    }

    public class Nothing : CVARCLowLevelCommand
    {

        public Nothing(Level2Client client)
        {
            Action = () =>
            {
                client.Move(0);
                client.Exit();
                return null;
            };
        }

        public override string ToString()
        {
            return "Nothing";
        }
    }
}
