using System;
using System.Collections.Generic;
using StrategyBuilder.SupportClasses;

namespace StrategyBuilder.Translation.CVARC
{
    public class CVARCTranslator : ITranslator
    {
        public List<LowLevelCommand> Translate(Report current, MovementTo action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var result = new List<LowLevelCommand>();

            var angleDelta = CVARCAngleCalculator.GetAngle(report.Coords, action.Coords, report.AngleInRadians);
            if (Math.Abs(angleDelta) > 0.0000001)
                result.Add(new Rotate((angleDelta/Math.PI)*180, report.Client));
            var length = Math.Sqrt(Math.Pow(action.Coords.X - report.Coords.X, 2) + Math.Pow(action.Coords.Y - report.Coords.Y, 2));
            result.Add(new Forward(length, report.Client));
            return result;
        }

        public List<LowLevelCommand> Translate(Report current, StoppingAt action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var angleDelta = CVARCAngleCalculator.GetAngle(report.Coords, action.Coords, report.AngleInRadians);
            var result = Translate(current, new MovementTo(action.Coords));
            if (Math.Abs(angleDelta) > 0.0001)
                result.Add(new Rotate((angleDelta/Math.PI)*180, report.Client));
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
