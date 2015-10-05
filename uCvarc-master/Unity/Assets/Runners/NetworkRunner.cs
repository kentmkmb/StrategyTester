using CVARC.V2;


namespace Assets
{
    public class NetworkRunner : IRunner
    {
        readonly CvarcClient client;
        readonly Configuration configuration;
        readonly ControllerFactory factory;
        readonly IWorldState worldState;

        public string Name { get; private set; }
        public IWorld World { get; private set; }
        public bool CanStart { get; private set; }
        public bool CanInterrupt { get; private set; }


        public NetworkRunner(CvarcClient client)
        {
            this.client = client;

            factory = new SoloNetworkControllerFactory(client);

            var configProposal = client.Read<ConfigurationProposal>();
            var loadingData = configProposal.LoadingData;
            var competitions = Dispatcher.Loader.GetCompetitions(loadingData);
            var settings = competitions.Logic.CreateDefaultSettings();
            if (configProposal.SettingsProposal != null)
                configProposal.SettingsProposal.Push(settings, true);
            configuration = new Configuration
            {
                LoadingData = loadingData,
                Settings = settings
            };

            //configuration.Settings.EnableLog = true;
            //configuration.Settings.LogFile = UnityConstants.LogFolderRoot + "cvarclog1";

            var worldSettingsType = competitions.Logic.WorldStateType;
            worldState = (IWorldState)client.Read(worldSettingsType);

            Name = loadingData.AssemblyName + loadingData.Level;
            CanInterrupt = true;
            CanStart = true;
        }

        public void InitializeWorld()
        {
            if (World == null)
                World = Dispatcher.Loader.CreateWorld(configuration, factory, worldState);
        }

        public void Dispose()
        {
            client.Close();
            if (World != null)
                World.OnExit();
        }
    }
}
