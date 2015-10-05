using System;
using RoboMovies;
using StrategyBuilder.Translation;

namespace StrategyBuilder.Translation.CVARC
{
    public class Move : LowLevelCommand
    {
        public double Speed;
        public double Time;

        public Move(double speed, double time)
        {
            Speed = speed;
            Time = time;
        }
    }

    public class Rotate : LowLevelCommand
    {
        public Action Action;

        public Rotate(double angleSpeed, double time)
        {

            //Action = new Action((() => ));
        }
    }

    public class Nothing : LowLevelCommand
    {
    }
}
