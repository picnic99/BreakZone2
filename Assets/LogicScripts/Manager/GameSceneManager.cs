using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
/// <summary>
/// 游戏场景管理类
/// </summary>
public class GameSceneManager : Manager<GameSceneManager>
{
    public static string LOAD_SCENE = "LOAD_SCENE";//加载场景
    public static string UNLOAD_SCENE = "UNLOAD_SCENE";//卸载场景

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
    
    /// <summary>
    /// 卸载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnLoadScene(string sceneName)
    {
        if (sceneName == null || sceneName.Length <= 0) return;
        GameScene scene = GetGameScene(sceneName);
        UnLoadScene(scene);
    }

    private void RemoveSceneDic(string name)
    {
        if (sceneDic.ContainsKey(name))
        {
            sceneDic.Remove(name);
        }
    }    
    private void AddSceneDic(GameScene scene)
    {
        sceneDic[scene.SceneName] = scene;
    }

    public void UnLoadScene(GameScene scene)
    {
        if (scene == null) return;
        if (scene != SwitchingScene)
        {
            scene.OnExit();
            GameSceneManager.Eventer.Event(UNLOAD_SCENE, scene.SceneName);
            RemoveSceneDic(scene.SceneName);
            //卸载相关内容
            /*            foreach (var item in GameContext.CurScene.SceneUIs)
                        {
                            UIManager.GetInstance().CloseUI(item);
                        }

                        foreach (var item in GameContext.CurScene.SceneCrts)
                        {
                            CharacterManager.GetInstance().RemoveCharacter(item);
                        }*/
        }
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //此处抛出进度
    }

    /// <summary>
    /// 场景加载完毕回调
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameContext.CurScene = SwitchingScene;
        UnLoadScene(GameContext.LastScene);
        SwitchingScene.OnEnter();
        AddSceneDic(SwitchingScene);
        GameSceneManager.Eventer.Event(LOAD_SCENE, SwitchingScene.SceneName);
    }
}
