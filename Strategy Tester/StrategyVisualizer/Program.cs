using System;
using System.Windows.Forms;
using StrategyBuilder;

namespace StrategyVisualizer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var config = new Config(70, Math.PI / 2);
            var planB = new Strategy()
                .MoveTo(300, 300)
                .MoveTo(400, 400)
                .StopAt(500, 500, 90)
                .End();
            var strategy = new Strategy(new Translator(config))
                .MoveTo(100, 100)
                .MoveTo(200, 100)
                .Else(planB)
                .MoveTo(200, 200)
                .StopAt(100, 200, 0)
                .End();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SimulationForm(strategy, new PointD(20, 20)));
        }
    }
}