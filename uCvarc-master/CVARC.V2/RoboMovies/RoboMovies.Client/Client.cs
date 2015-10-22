using System;
using System.Threading;
using System.Windows.Forms;
using CVARC.V2;
using RoboMovies;
using StrategyBuilder;
using StrategyBuilder.Translation.CVARC;

namespace ClientExample
{
    class Program
    {

        static void PrintLocation(CommonSensorData sensors)
        {
            var location = sensors.SelfLocation;
            Console.WriteLine(@"{0} {1}", location.X, location.Y);
        }

        static ClientForm form;

        static void Control(int port, IStrategy strategy)
        {
            var client = new Level2Client();
			client.SensorDataReceived += sensorData => form.ShowMap(sensorData.Map);
			var startInfo = client.Configurate(port, true, RoboMoviesBots.Stand);
            var currentReport = new CVARCReport(startInfo, client, true);
            PrintLocation(startInfo);
            while (true)
            {
                var newAction = strategy.GetNextState(currentReport);
                foreach (var command in newAction.Item1)
                {
                    var cvarccommand = command as CVARCLowLevelCommand;
                    var newState = cvarccommand.Action();
                    if (cvarccommand is Nothing)
                        return;
                    currentReport = new CVARCReport(newState, client, true);
                }
            }
        }

        static void Run(int port)
        {
            var strategy = new Strategy(new CVARCTranslator()).MoveTo(-60, -55).End();
            form = new ClientForm();
			new Thread(
				() =>
				{
					Control(port, strategy);
				}).Start();
            Application.Run(form);
        }
		
        [STAThread]
        public static void Main(string[] args)
        {
            CVARCProgram.RunServerInTheSameThread(Run);
        }
    }
}