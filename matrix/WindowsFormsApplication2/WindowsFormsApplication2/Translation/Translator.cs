using System;
using System.Collections.Generic;
using System.Drawing;

namespace StrategyTester
{
    public class Translator : ITranslator
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

        public Translator(
            double maxLinearSpeed, 
            double maxAngleSpeed, 
            double linearSpeedCoefficient = 1, 
            double angleSpeedCoefficient = 1
            )
        {
            robotInfo.MaxLinearSpeed = maxLinearSpeed;
            robotInfo.MaxAngleSpeed = maxAngleSpeed;
            LinearSpeedСoefficient = linearSpeedCoefficient;
            AngleSpeedСoefficient = angleSpeedCoefficient;
        }
        public Translator(Config config) : this(config.RobotMaxLinearSpeed, config.RobotMaxAngleSpeed) { }

        private double CalculateAngleDelta(double currentAngle, double targetAngle)
        {
            var delta = targetAngle - currentAngle;
            if (Math.Abs(delta) < Math.PI) return delta;
            else return -Math.Sign(delta) * (2 * Math.PI - Math.Abs(delta));
        }
        private Rotate MakeRotate(double currentAngle, double targetAngle)
        {
            if (currentAngle == targetAngle) throw new ArgumentException();
            var speed = AngleSpeedСoefficient * robotInfo.MaxAngleSpeed;
            var delta = CalculateAngleDelta(currentAngle, targetAngle);
            return new Rotate(Math.Sign(delta) * speed, Math.Abs(delta) / speed);
        }
        private Forward MakeForward(MyPoint current, MyPoint target)
        {
            var speed = LinearSpeedСoefficient * robotInfo.MaxLinearSpeed;
            var length = Math.Sqrt(Math.Pow(target.X - current.X, 2) + Math.Pow(target.Y - current.Y, 2));
            return new Forward(speed, length / speed);
        }
        public List<LowLevelCommand> Translate(Report current, MovementTo action)
        {
            var result = new List<LowLevelCommand>();
            var targetAngle = AngleCaculator.CalculateAngle(current.Coords, action.Coords);
            if (targetAngle != current.AngleInRadians) 
                result.Add(MakeRotate(current.AngleInRadians, targetAngle));
            result.Add(MakeForward(current.Coords, action.Coords));
            return result;
        }
        public List<LowLevelCommand> Translate(Report current, StoppingAt action)
        {
            var targetAngle = AngleCaculator.CalculateAngle(current.Coords, action.Coords);
            var result = Translate(current, new MovementTo(action.Coords));
            result.Add(MakeRotate(targetAngle, action.AngleInRadians));
            return result;
        }
        public List<LowLevelCommand> Translate(Report current, EndOfStrategy action)
        {
            var result = new List<LowLevelCommand>();
            result.Add(new Nothing());
            return result;
        }
    }
}