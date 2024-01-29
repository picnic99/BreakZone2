using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Manager<GameSceneManager>
{
    public List<GameScene> scenes;

    public GameScene SwitchingScene;

    public override void Init()
    {
        scenes = new List<GameScene>();
        SceneManager.sceneLoaded += SceneLoaded;
        base.Init();
    }

    public void SwitchScene(string sceneName)
    {
        Type type = RegSceneClass.GetInstance().Get(sceneName);
        if (type != null)
        {
            GameScene scene = (GameScene)type.Assembly.CreateInstance(type.Name);
            SwitchingScene = scene;
            if (GameContext.CurScene != null && GameContext.CurScene != SwitchingScene)
            {
                GameContext.CurScene.OnExit();
                //移除上个场景的UI
                foreach (var item in GameContext.CurScene.SceneUIs)
                {
                    UIManager.GetInstance().CloseUI(item);
                }

                foreach (var item in GameContext.CurScene.SceneCrts)
                {
                    CharacterManager.GetInstance().RemoveCharacter(item);
                }
            }
            LoadScene(sceneName);
        }

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DoneCall();
    }

    private void DoneCall()
    {
        SwitchingScene.OnEnter();
    }
}
