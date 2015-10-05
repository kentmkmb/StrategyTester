using System;
using CVARC.V2;
using RoboMovies;
using UnityEngine;
using Assets;


public static class Dispatcher
{
    public static Loader Loader { get; private set; }
    public static IRunner CurrentRunner { get; private set; }
    public static bool UnityShutdown { get; private set; }
    static readonly RunnersQueue queue = new RunnersQueue();
    static bool isGameOver;
    static UnityServer networkServer;
    static UnityServer torunamentServer;
    static bool switchingScenes;


    public static void Start()
    {
        Debugger.DisableByDefault = true;
        Debugger.EnabledTypes.Add(DebuggerMessageType.Unity);
        Debugger.EnabledTypes.Add(DebuggerMessageType.UnityTest);
        //Debugger.EnabledTypes.Add(RMDebugMessage.WorldCreation);
        //Debugger.EnabledTypes.Add(RMDebugMessage.Logic);
        //Debugger.EnabledTypes.Add(RMDebugMessage.Workflow);
        //Debugger.EnabledTypes.Add(DebuggerMessageType.Workflow);
        Debugger.Logger = Debug.Log;

        Loader = new Loader();
        Loader.AddLevel("Demo", "Test", () => new DemoCompetitions.Level1());
        Loader.AddLevel("RoboMovies", "Test", () => new RMCompetitions.Level1());

        networkServer = new SoloNetworkServer(UnityConstants.SoloNetworkPort);
        Action networkServerAction = () => networkServer.StartThread();
        networkServerAction.BeginInvoke(null, null);

        torunamentServer = new TournamentServer(UnityConstants.TournamentPort);
        Action tournamentServerAction = () => torunamentServer.StartThread();
        tournamentServerAction.BeginInvoke(null, null);
    }

    public static void AddRunner(IRunner runner)
    {
        queue.EnqueueRunner(runner);
    }

    public static void IntroductionTick()
    {
        if (queue.HasReadyRunner())
            SwitchScene("Round");
    }

    public static void RoundStart()
    {
        CurrentRunner = queue.DequeueReadyRunner();
        isGameOver = false;
        CurrentRunner.InitializeWorld();
    }

    public static void RoundTick()
    {
        // конец игры
        if (isGameOver && CurrentRunner != null)
        {
            Debug.Log("game over. disposing");
            CurrentRunner.Dispose();
            CurrentRunner = null;
        }

        if (CurrentRunner == null)
        {
            // очищение, или переход в начало
            SwitchScene(queue.HasReadyRunner() ? "Round" : "Intro");
            return;
        }

        // прерывание
        if (queue.HasReadyRunner() && CurrentRunner.CanInterrupt)
            SetGameOver();
    }

    // самый ГЛОБАЛЬНЫЙ выход, из всей юнити. Вызывается из сцен.
    public static void OnDispose()
    {
        if (switchingScenes)
        {
            switchingScenes = false;
            return;
        }

        Debug.Log("GLOBAL EXIT");
        networkServer.RequestExit();
        torunamentServer.RequestExit();
        if (CurrentRunner != null)
            CurrentRunner.Dispose();
        queue.DisposeRunners();

        UnityShutdown = true;
    }

    public static void SetGameOver()
    {
        isGameOver = true;
    }

    static void SwitchScene(string sceneName)
    {
        switchingScenes = true;
        Application.LoadLevel(sceneName);
    }
}
