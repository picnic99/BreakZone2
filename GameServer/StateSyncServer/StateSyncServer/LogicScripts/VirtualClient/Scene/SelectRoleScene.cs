using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateSyncServer.LogicScripts.VirtualClient.Scene
{
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
            CameraManager.GetInstance().CloseCam();
        }

    }
}