using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateSyncServer.LogicScripts.VirtualClient.Scene
{
    public class LoginScene : GameScene
    {
        public LoginScene()
        {
            SceneName = RegSceneClass.LoginScene;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //º”‘ÿUI
            UIManager.GetInstance().ShowUI(RegUIClass.LoginUI);
            CameraManager.GetInstance().CloseCam();
        }

    }
}