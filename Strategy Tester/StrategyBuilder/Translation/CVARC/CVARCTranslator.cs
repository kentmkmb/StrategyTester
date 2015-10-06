using System;
using System.Collections.Generic;

namespace StrategyBuilder.Translation.CVARC
{
    public class CVARCTranslator : ITranslator
    {
        private double CalculateAngleDelta(double currentAngle, double targetAngle)
        {
            var delta = targetAngle - currentAngle;
            if (Math.Abs(delta) < Math.PI) return delta;
            return -Math.Sign(delta) * (2 * Math.PI - Math.Abs(delta));
        }

        public List<LowLevelCommand> Translate(Report current, MovementTo action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var result = new List<LowLevelCommand>();
            var targetAngle = AngleCaculator.CalculateAngle(report.Coords, action.Coords);
            if (Math.Abs(targetAngle - report.AngleInRadians) > 0.0001)
            {
                var delta = CalculateAngleDelta(report.AngleInRadians, targetAngle);
                result.Add(new Rotate((delta / Math.PI) * 180, report.Client));
            }
            var length = Math.Sqrt(Math.Pow(action.Coords.X - report.Coords.X, 2) + Math.Pow(action.Coords.Y - report.Coords.Y, 2));
            result.Add(new Forward(length, report.Client));
            return result;
        }

        public List<LowLevelCommand> Translate(Report current, StoppingAt action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var targetAngle = AngleCaculator.CalculateAngle(report.Coords, action.Coords);
            var result = Translate(current, new MovementTo(action.Coords));
            if (Math.Abs(targetAngle - report.AngleInRadians) > 0.0001)
            {
                var delta = CalculateAngleDelta(report.AngleInRadians, targetAngle);
                result.Add(new Rotate((delta/Math.PI)*180, report.Client));
            }
            return result;
        }

        public List<LowLevelCommand> Translate(Report current, EndOfStrategy action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var result = new List<LowLevelCommand> { new Nothing(report.Client) };
            return result;
        }
    }
}
