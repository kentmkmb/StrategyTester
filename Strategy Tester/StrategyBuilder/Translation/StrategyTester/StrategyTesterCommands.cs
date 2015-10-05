namespace StrategyBuilder.Translation.StrategyTester
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

    public class Nothing : LowLevelCommand
    {
        public override string ToString()
        {
            return "Nothing";
        }
    }
}
