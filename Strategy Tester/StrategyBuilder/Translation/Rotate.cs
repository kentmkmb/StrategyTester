
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

        public override string ToString()
        {
            return string.Format("Rotate({0}, {1})", AngleSpeed, Time);
        }
    }
}
