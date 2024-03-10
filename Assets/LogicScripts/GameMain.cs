using Assets.LogicScripts.Client.Net.PB;
using Assets.LogicScripts.Client;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Assets.LogicScripts.Client.Manager;

public class GameMain : MonoBehaviour
{
    private static GameMain _instance;
    public static GameMain GetInstance()
    {
        return _instance;
    }

    public List<IManager> managers;
    public Queue<Protocol> ProtoList = new Queue<Protocol>();


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        RegProtocol.Init();
        managers = new List<IManager>()
        {
            ResourceManager.GetInstance(),
            UIManager.GetInstance(),
            DebugManager.GetInstance(),
            InputManager.GetInstance(),
            CharacterManager.GetInstance(),
            GameSceneManager.GetInstance(),
            CameraManager.GetInstance(),
        };

        Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().Init();
        SceneManager.GetInstance().Init();
        //CharacterManager.GetInstance().Init();

        GameStart();
    }

    void Update()
    {
        RegSystemHotKey();
        //协议处理
        while (ProtoList.Count > 0)
        {
            var protocol = ProtoList.Dequeue();
            Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().Event(protocol.protocolId, protocol);
        }
        UpdateManager();
    }


    public void UpdateManager()
    {
        Assets.LogicScripts.Client.InputManager.GetInstance().OnUpdate();
        foreach (var item in managers)
        {
            item.OnUpdate();
        }
    }

    public void GameStart()
    {
        Assets.LogicScripts.Client.Manager.NetManager.GetInstance().Connect();
        GameSceneManager.GetInstance().SwitchScene(RegSceneClass.LoginScene);
    }

    public void RegSystemHotKey()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            DebugManager.GetInstance().ShowPanel();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var item in GameContext.CurScene.SceneCrts)
            {
                CharacterManager.GetInstance().RemoveCharacter(item);
            }
        }
    }
} 
