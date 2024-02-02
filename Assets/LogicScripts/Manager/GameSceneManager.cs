using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Manager<GameSceneManager>
{
    /// <summary>
    /// 场景与场景名称映射
    /// </summary>
    public Dictionary<string, GameScene> sceneDic;
    /// <summary>
    /// 当前处于切换中的场景
    /// </summary>
    public GameScene SwitchingScene;

    public override void Init()
    {
        sceneDic = new Dictionary<string, GameScene>();
        SceneManager.sceneLoaded += SceneLoaded;
        base.Init();
    }


    /// <summary>
    /// 切换场景
    /// 1、卸载旧的场景 同时通知所有与场景相关的物体卸载
    /// 2、加载新场景 走新场景相关逻辑
    /// </summary>
    /// <param name="sceneName"></param>
    public void SwitchScene(string sceneName)
    {
        Type type = RegSceneClass.GetInstance().Get(sceneName);
        if (type != null)
        {
            GameScene scene = (GameScene)type.Assembly.CreateInstance(type.Name);
            SwitchingScene = scene;
            UnLoadScene(GameContext.LastScene);
            LoadScene(sceneName);
        }

    }

    /// <summary>
    /// 获取游戏场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public GameScene GetGameScene(string sceneName)
    {
        if (sceneDic.ContainsKey(sceneName))
        {
            return sceneDic[sceneName];
        }
        return null;
    }

    public void UnLoadScene(string sceneName)
    {
        if (sceneName == null || sceneName.Length <= 0) return;
        GameScene scene = GetGameScene(sceneName);
        UnLoadScene(scene);
    }

    public void UnLoadScene(GameScene scene)
    {
        if (scene == null) return;
        if (scene != SwitchingScene)
        {
            GameContext.CurScene.OnExit();
            //卸载相关内容
            foreach (var item in GameContext.CurScene.SceneUIs)
            {
                UIManager.GetInstance().CloseUI(item);
            }

            foreach (var item in GameContext.CurScene.SceneCrts)
            {
                CharacterManager.GetInstance().RemoveCharacter(item);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //此处抛出进度
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwitchingScene.OnEnter();
    }
}
