
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
    }
}
