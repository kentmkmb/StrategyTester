using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EurobotStrategyCreator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var planB = new Strategy()
                .MoveTo(300, 300)
                .MoveTo(400, 400)
                .StopAt(500, 500, 90)
                .End();
            var strategy = new Strategy()
                .MoveTo(100, 100)
                .MoveTo(200, 100)
                .Else(planB)
                .MoveTo(200, 200)
                .StopAt(100, 200, 0)
                .End();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(strategy, new Report(0, new Point(20, 20), true)));
        }
    }
}
