using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoleScene : GameScene
{
    public SelectRoleScene()
    {
        SceneName = RegSceneClass.SelectRoleScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //����UI
        UIManager.GetInstance().ShowUI(RegUIClass.SelectRoleUI);
    }

}
