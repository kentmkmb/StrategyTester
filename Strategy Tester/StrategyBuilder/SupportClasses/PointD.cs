namespace StrategyBuilder
{
    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("Point({0}, {1})", X, Y);
        }
    }
}
