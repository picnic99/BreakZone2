using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : GameScene
{
    public LoginScene()
    {
        SceneName = RegSceneClass.LoginScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //����UI
        UIManager.GetInstance().ShowUI(RegUIClass.LoginUI);
        CameraManager.GetInstance().CloseCam();
    }

}
