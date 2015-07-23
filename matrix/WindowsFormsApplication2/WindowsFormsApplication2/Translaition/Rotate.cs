using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    public enum RotationDirection
    {
        Сlock,
        CounterClock
    }
    public class Rotate : LowLevelCommand
    {
        public RotationDirection Direction;
        public double AngleSpeed;
        public double Time;
    }
}
