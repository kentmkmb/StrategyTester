using CVARC.V2;


namespace Assets
{
    class TournamentServer : UnityServer
    {
        public TournamentServer(int port) : base(port) { }

        protected override void HandleClient(CvarcClient client)
        {
            TournamentPool.AddPlayerToPool(client);
        }

        protected override void Print(string str)
        {
            Debugger.Log(DebuggerMessageType.Unity, "tournament server: " + str);
        }
    }
}
