using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : Base
{
    public string SceneName;

    public List<string> SceneUIs = new List<string>();

    public virtual void OnEnter()
    {
        GameContext.CurScene = this;
    }

    public virtual void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }
}
