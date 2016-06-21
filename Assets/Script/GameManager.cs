﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;
//游戏的入口，不可销毁
public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    [HideInInspector]
    public LuaManager luaMgr = null;
    [HideInInspector]
    public NetworkManager netMgr = null;
    [HideInInspector]
    public ResManager resMgr = null;
    [HideInInspector]
    public UIManager uiMgr = null;

    public static Entity MainPlayer = null;

    public uint HeroConfigID = 10;

    public static Process Proc;
    void Awake()
    {
        Proc = Process.GetCurrentProcess();
        Application.targetFrameRate = 45;
        Instance = this;
        DontDestroyOnLoad(gameObject);   
        luaMgr = gameObject.AddComponent<LuaManager>();
        netMgr = gameObject.AddComponent<NetworkManager>();
        resMgr = gameObject.AddComponent<ResManager>();
        uiMgr = gameObject.AddComponent<UIManager>();
    }
	// Use this for initialization
	void Start () {

        AStar.LoadPathInfo("PathInfo/main");
        luaMgr.InitStart();

        CSVManager.LoadAllCsv();
        MainPlayer = EntityManager.Instance.Get(HeroConfigID, 1);
        MainPlayer.Camp = eCamp.Hero;

        Entity enemy = EntityManager.Instance.Get(10, 2);
        enemy.Pos = new Vector3(25f, 4.6f, 42f);
        enemy.Camp = eCamp.Enemy;

        MainPlayer.Pos = new Vector3(28.5f, 4.6f, 45f);
        CameraController.Instance.LookTarget = MainPlayer;
        //MainPlayer.AttachCamera();

        //UIManager.Instance.CreatePanel("FightPanel");
	}
	
	// Update is called once per frame
	void Update ()
    {
        EntityManager.Instance.Update();
        EffectManager.Instance.Update();
        BulletManager.Instance.Update();

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            MainPlayer.Skill.CastSkill(0);
        }
	}

    void OnApplicationQuit()
    {
        CSVManager.ClearAllCsv();
        EntityManager.Instance.Clear();
        EffectManager.Instance.Clear();
        BulletManager.Instance.Clear();
        Log.Info("application quit.");
        Log.Stop();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 30), "当前内存 : " + Proc.PrivateMemorySize64);
    }
}
