
namespace StrategyBuilder
{
    public class Forward : LowLevelCommand
    {
        public double Speed;
        public double Time;

        public Forward(double speed, double time)
        {
            Speed = speed;
            Time = time;
        }

        public override string ToString()
        {
            return string.Format("Forward({0}, {1})", Speed, Time);
        }
    }
}
