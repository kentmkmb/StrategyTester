
namespace StrategyBuilder
{
    public class Rotate : LowLevelCommand
    {
        public double AngleSpeed;
        public double Time;

        public Rotate(double angleSpeed, double time)
        {
            AngleSpeed = angleSpeed;
            Time = time;
        }
    }
}
