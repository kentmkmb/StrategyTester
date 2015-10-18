using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyBuilder;
using StrategyVisualizer;

namespace StrategyBuilderTester
{
    [TestClass]
    public class StrategyTest
    {
        public class DoubleComaperer : IComparer
        {
            public int Compare(object x, object y)
            {
                var dx = (double) x;
                var dy = (double) y;
                return Math.Abs(dx - dy) < 0.0001 ? 0 : dx.CompareTo(dy);
            }
        }

        public void TestStrategy(Strategy strategy, World environment, StrategyTesterReport start, List<PointD> expectedPoints, List<double> expectedAngles, List<bool> expectedResults)
        {
            var curReport = start;
            var reports = new List<StrategyTesterReport>();
            while (true)
            {
                var newAction = strategy.GetNextState(curReport);
                var movesList = newAction.Item1;
                if (newAction.Item2 is EndOfStrategy) break;
                curReport = environment.TryMove(curReport, movesList, true);
                reports.Add(curReport);
            }
            CollectionAssert.AreEqual(reports.Select(x => x.Coords).ToList(), expectedPoints);
            CollectionAssert.AreEqual(reports.Select(x => x.AngleInRadians).ToList(), expectedAngles, new DoubleComaperer());
            CollectionAssert.AreEqual(reports.Select(x => x.Success).ToList(), expectedResults);
        }

        [TestMethod]
        public void TestStrategyWithoutObstacles()
        {
            var strategy = new Strategy(new StrategyTesterTranslator())
                .MoveTo(100, 100)
                .MoveTo(200, 100)
                .MoveTo(200, 200)
                .StopAt(100, 200, 0)
                .End();
            var start = new StrategyTesterReport(0, new PointD(20, 20), true);
            TestStrategy(
                strategy,
                new World(start.Coords, 0),
                start,
                new List<PointD>
                {
                    new PointD(100, 100),
                    new PointD(200, 100),
                    new PointD(200, 200),
                    new PointD(100, 200)
                },
                new List<double> { Math.PI * 2 - Math.PI / 4, 0, Math.PI * 2 - Math.PI / 2, 0 },
                new List<bool> { true, true, true, true }
                );
        }

        [TestMethod]
        public void TestStrategyWithObstacles()
        {
            var planB = new Strategy()
                .MoveTo(300, 300)
                .MoveTo(400, 400)
                .StopAt(500, 500, 90)
                .End();
            var strategy = new Strategy(new StrategyTesterTranslator())
                .MoveTo(100, 100)
                .MoveTo(200, 100)
                .Else(planB)
                .MoveTo(200, 200)
                .StopAt(100, 200, 0)
                .End();
            var start = new StrategyTesterReport(0, new PointD(20, 20), true);
            var environment = new World(start.Coords, 0);
            environment.Objects.Add(new Polygon(new Point(150, 90), new Size(10, 20)));
            TestStrategy(
                strategy,
                environment,
                start,
                new List<PointD>
                {
                    new PointD(100, 100),
                    new PointD(100, 100),
                    new PointD(300, 300),
                    new PointD(400, 400),
                    new PointD(500, 500)
                },
                new List<double>
                {
                    Math.PI * 2 - Math.PI / 4,
                    Math.PI * 2 - Math.PI / 4,
                    Math.PI * 2 - Math.PI / 4,
                    Math.PI * 2 - Math.PI / 4,
                    Math.PI / 2
                },
                new List<bool> { true, false, true, true, true }
            );
            environment = new World(start.Coords, 0);
            environment.Objects.Add(new Polygon(new Point(182, 140), new Size(30, 10)));
            strategy.GoToPreviousState(6);
            TestStrategy(
                strategy,
                environment,
                start,
                new List<PointD>
                {
                    new PointD(100, 100),
                    new PointD(200, 100),
                    new PointD(200, 100),
                    new PointD(300, 300),
                    new PointD(400, 400),
                    new PointD(500, 500)
                },
                new List<double>
                {
                    Math.PI * 2 - Math.PI / 4,
                    0,
                    0,
                    AngleCaculator.CalculateAngle(new PointD(200, 100), new PointD(300, 300)),
                    Math.PI * 2 - Math.PI / 4,
                    Math.PI / 2
                },
                new List<bool> { true, true, false, true, true, true }
            );
        }
    }
}