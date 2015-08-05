﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace StrategyBuilder
{
    public class StoppingAt : State
    {
        public PointD Coords;
        public double AngleInRadians;

        public StoppingAt(double angle, PointD coords)
        {
            AngleInRadians = angle;
            Coords = coords;
            this.Next = null;
            this.Previous = null;
            this.Alternative = null;
        }
        public override string ToString()
        {
            return String.Format("StopAt({0}, {1}, {2})", Coords.X, Coords.Y, AngleInRadians);
        }
        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            return translator.Translate(current, this);
        }
    }
}
