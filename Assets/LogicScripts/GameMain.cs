using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    private static GameMain _instance;
    public static GameMain GetInstance()
    {
        return _instance;
    }

    public List<IManager> managers;

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
        GameStart();
    }

    void Update()
    {
        RegSystemHotKey();
        UpdateManager();
    }


    public void UpdateManager()
    {
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
