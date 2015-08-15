using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyBuilder;

namespace StrategyBuilderTester
{
    [TestClass]
    public class AngleCalculatorTest
    {
        [TestMethod]
        public void Left()
        {
            Assert.AreEqual(
                Math.PI,
                AngleCaculator.CalculateAngle(new PointD(0, 0), new PointD(-1, 0))
                );
        }
        [TestMethod]
        public void Right()
        {
            Assert.AreEqual(
                0,
                AngleCaculator.CalculateAngle(new PointD(0, 0), new PointD(1, 0))
                );
        }
        [TestMethod]
        public void Up()
        {
            Assert.AreEqual(
                Math.PI / 2,
                AngleCaculator.CalculateAngle(new PointD(0, 0), new PointD(0, -1))
                );
        }
        [TestMethod]
        public void Down()
        {
            Assert.AreEqual(
                3 * Math.PI / 2,
                AngleCaculator.CalculateAngle(new PointD(0, 0), new PointD(0, 1))
                );
        }
        [TestMethod]
        public void FourtyFiveDegrees()
        {
            Assert.AreEqual(
                Math.PI / 4,
                AngleCaculator.CalculateAngle(new PointD(0, 0), new PointD(1, -1))
                );
        }
        [TestMethod]
        public void ThirdtyDegrees()
        {
            Assert.AreEqual(
                Math.PI / 6,
                AngleCaculator.CalculateAngle(new PointD(0, 0), new PointD(1, -1 / Math.Sqrt(3)))
                );
        }
    }
}
