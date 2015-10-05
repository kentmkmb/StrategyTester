using System.Collections.Generic;
using Assets;
using CVARC.V2;

namespace Assets
{
    public static class TournamentPool
    {
        static readonly Dictionary<LoadingData, TournamentRunner> pool = new Dictionary<LoadingData, TournamentRunner>();


        public static void AddPlayerToPool(CvarcClient client)
        {
            var configProposal = client.Read<ConfigurationProposal>();
            var loadingData = configProposal.LoadingData;
            var competitions = Dispatcher.Loader.GetCompetitions(loadingData);
            var worldSettingsType = competitions.Logic.WorldStateType;
            var worldState = (IWorldState)client.Read(worldSettingsType); 
            

            lock (pool)
            {
                if (!pool.ContainsKey(loadingData))
                {
                    pool.Add(loadingData, new TournamentRunner(loadingData, worldState));
                    Dispatcher.AddRunner(pool[loadingData]);
                }
                if (pool[loadingData].AddPlayerAndCheck(new TournamentPlayer(client, configProposal, worldState)))
                    pool.Remove(loadingData);
            }
        }
    }
}
