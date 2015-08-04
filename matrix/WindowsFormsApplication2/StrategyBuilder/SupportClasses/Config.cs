﻿
namespace StrategyTester
{
    public class Config
    {
        public double RobotMaxLinearSpeed;
        public double RobotMaxAngleSpeed;

        public Config(double robotMaxLinearSpeed, double robotMaxAngleSpeed)
        {
            this.RobotMaxLinearSpeed = robotMaxLinearSpeed;
            this.RobotMaxAngleSpeed = robotMaxAngleSpeed;
        }
    }
}