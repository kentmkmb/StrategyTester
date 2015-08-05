
namespace StrategyBuilder
{
    public class Config
    {
        public double RobotMaxLinearSpeed;
        public double RobotMaxAngleSpeed;

        public Config(double robotMaxLinearSpeed, double robotMaxAngleSpeed)
        {
            RobotMaxLinearSpeed = robotMaxLinearSpeed;
            RobotMaxAngleSpeed = robotMaxAngleSpeed;
        }
    }
}
