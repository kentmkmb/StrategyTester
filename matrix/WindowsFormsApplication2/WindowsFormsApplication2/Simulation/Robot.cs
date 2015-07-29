using System.Drawing;

namespace StrategyTester
{
    class Robot
    {
        public Point Coords;
        public double Angle;
        public double Size;

        public Robot(Point coords, double angle)
        {
            Coords = coords;
            Angle = angle;
        }
    }
}
