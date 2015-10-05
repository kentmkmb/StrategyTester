using System;
using System.Collections.Generic;
using StrategyBuilder.Translation;
using StrategyBuilder.Translation.StrategyTester;

namespace StrategyBuilder
{
    public class StrategyTesterTranslator : ITranslator
    {
        struct RobotInfo
        {
            public double MaxLinearSpeed;
            public double MaxAngleSpeed;
        }
        private RobotInfo robotInfo;

        private double linearSpeedСoefficient;
        public double LinearSpeedСoefficient
        {
            get { return linearSpeedСoefficient; }
            set 
            {
                if ((value > 1) || (value <= 0)) throw new ArgumentException();
                linearSpeedСoefficient = value;
            }
        }
        private double angleSpeedСoefficient;
        public double AngleSpeedСoefficient
        {
            get { return angleSpeedСoefficient; }
            set
            {
                if ((value > 1) || (value <= 0)) throw new ArgumentException();
                angleSpeedСoefficient = value;
            }
        }

        public StrategyTesterTranslator()
        {
            robotInfo.MaxLinearSpeed = Config.RobotMaxLinearSpeed;
            robotInfo.MaxAngleSpeed = Config.RobotMaxAngleSpeed;
            LinearSpeedСoefficient = 1;
            AngleSpeedСoefficient = 1;
        }
        private double CalculateAngleDelta(double currentAngle, double targetAngle)
        {
            var delta = targetAngle - currentAngle;
            if (Math.Abs(delta) < Math.PI) return delta;
            else return -Math.Sign(delta) * (2 * Math.PI - Math.Abs(delta));
        }
        private Rotate MakeRotate(double currentAngle, double targetAngle)
        {
            if (Math.Abs(currentAngle - targetAngle) < 0.00001) throw new ArgumentException();
            var speed = AngleSpeedСoefficient * robotInfo.MaxAngleSpeed;
            var delta = CalculateAngleDelta(currentAngle, targetAngle);
            return new Rotate(Math.Sign(delta) * speed, Math.Abs(delta) / speed);
        }
        private Forward MakeForward(PointD current, PointD target)
        {
            var speed = LinearSpeedСoefficient * robotInfo.MaxLinearSpeed;
            var length = Math.Sqrt(Math.Pow(target.X - current.X, 2) + Math.Pow(target.Y - current.Y, 2));
            return new Forward(speed, length / speed);
        }
        public List<LowLevelCommand> Translate(Report current, MovementTo action)
        {
            var report = current as StrategyTesterReport;
            if (report == null) 
                throw new ArgumentException();
            var result = new List<LowLevelCommand>();
            var targetAngle = AngleCaculator.CalculateAngle(report.Coords, action.Coords);
            if (Math.Abs(targetAngle - report.AngleInRadians) > 0.0001)
                result.Add(MakeRotate(report.AngleInRadians, targetAngle));
            result.Add(MakeForward(report.Coords, action.Coords));
            return result;
        }
        public List<LowLevelCommand> Translate(Report current, StoppingAt action)
        {
            var report = current as StrategyTesterReport;
            if (report == null)
                throw new ArgumentException();
            var targetAngle = AngleCaculator.CalculateAngle(report.Coords, action.Coords);
            var result = Translate(current, new MovementTo(action.Coords));
            result.Add(MakeRotate(targetAngle, action.AngleInRadians));
            return result;
        }
        public List<LowLevelCommand> Translate(Report current, EndOfStrategy action)
        {
            var result = new List<LowLevelCommand> { new Nothing() };
            return result;
        }
    }
}