using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : Base
{
    public string SceneName;
    public UIBinding bind;

    public List<string> SceneUIs = new List<string>();
    public List<Character> SceneCrts = new List<Character>();

    public virtual void OnEnter()
    {
        bind = GameObject.Find("SceneData")?.GetComponent<UIBinding>();
    }

    public virtual void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public void AddCrtToScene()
    {

    }
}
