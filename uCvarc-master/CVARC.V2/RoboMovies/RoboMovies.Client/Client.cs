using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoboMovies;
using StrategyBuilder.Translation.CVARC;

namespace ClientExample
{
    class Program
    {

        static void PrintLocation(CommonSensorData sensors)
        {
            var location = sensors.SelfLocation;
            Console.WriteLine("{0} {1}", location.X, location.Y);
        }

        static ClientForm form;

        static void Control(int port)
        {
            var client = new Level2Client();
			client.SensorDataReceived += sensorData => form.ShowMap(sensorData.Map);
			client.Configurate(port, true, RoboMoviesBots.Stand);
            
			client.Rotate(-90);
            client.Move(100);
            client.Rotate(90);
            client.Move(110);
            for (int i = 0; i < 10; i++)
            {
                client.Rotate(10);
            }
            client.Exit();
        }

        static void Run(int port)
        {
            var strategy = new StrategyBuilder.Strategy(new CVARCTranslator());
            form = new ClientForm();
			new Thread(
				() =>
				{
					Control(port);
					return;
				}).Start();
            Application.Run(form);
        }

		
        [STAThread]
        public static void Main(string[] args)
        {
            CVARC.V2.CVARCProgram.RunServerInTheSameThread(Run);
        }

    }
}