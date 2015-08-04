using StrategyBuilder;

namespace StrategyVisualizer
{
    class Robot
    {
        public MyPoint Coords;
        public double Angle;
        public double Size;

        public Robot(MyPoint coords, double angle)
        {
            Coords = coords;
            Angle = angle;
        }
    }
}
