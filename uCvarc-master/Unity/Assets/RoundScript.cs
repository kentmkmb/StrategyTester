using UnityEngine;
using Assets;
using CVARC.V2;
using AIRLab;


public partial class RoundScript : MonoBehaviour
{
    public static RoundScript Behaviour { get; private set; }
    public static Tuple<string, string, int> CollisionInfo { get; set; }
    IWorld world;
    GUIText scoresTextLeft;
    GUIText scoresTextRight;
    GameObject myCamera;
    public GameObject cubePref; // Эти поля -- прототипы, к ним самим обращаться не получится.
    bool worldPrepearedToExit;
    float curWorldTime;
    float timeOnStartSession;
    private long lastStart;


    void Start()
    {
        Dispatcher.RoundStart();

        timeOnStartSession = Time.fixedTime;
        curWorldTime = 0;
        Behaviour = this;
        CameraCreator();
        ScoresFieldsCreator();

        world = Dispatcher.CurrentRunner.World;
        if (world != null)
            Debugger.Log(DebuggerMessageType.Unity, "World loaded");
        else
            Debugger.Log(DebuggerMessageType.Unity, "Fail. World not loaded");

        CollisionInfo = new Tuple<string, string, int>(null, null, 0);
        Time.timeScale = 1; // вот почему так?
        //в момент повторного запуска время уже не нулевое
    }

    void Update()
    {
        Dispatcher.RoundTick();

        if (curWorldTime > 30)
        {
            Debugger.Log(DebuggerMessageType.Unity,"Time is Up");
            Dispatcher.SetGameOver();
            return;
        }
        
        if (CollisionInfo.Item3 == 2)
        {
            ((UEngine)world.Engine).CollisionSender(CollisionInfo.Item1, CollisionInfo.Item2);
            CollisionInfo.Item3 = 0;
        }

        UpdateScores();
    }

    void FixedUpdate() //только физика и строгие расчеты. вызывается строго каждые 20 мс
    {
        curWorldTime = Time.fixedTime - timeOnStartSession;
        world.Clocks.Tick(curWorldTime);
        ((UEngine)world.Engine).UpdateSpeeds();
    }

    void OnDisable()
    {
        Dispatcher.OnDispose();
    }
}