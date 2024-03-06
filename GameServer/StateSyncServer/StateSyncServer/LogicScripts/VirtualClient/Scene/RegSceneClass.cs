using StateSyncServer.LogicScripts.VirtualClient.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateSyncServer.LogicScripts.VirtualClient.Scene
{
    public class RegSceneClass : RegisterBase<RegSceneClass, string, Type>
    {
        public static string SelectRoleScene = "SelectRole";
        public static string LoginScene = "Login";
        public static string GameRoomScene = "GameRoom";
        public static string AllScene = "AllScene";

        public override void Init()
        {
            base.Init();
            regDic.Add(SelectRoleScene, typeof(SelectRoleScene));
            regDic.Add(GameRoomScene, typeof(GameRoomScene));
            regDic.Add(LoginScene, typeof(LoginScene));
        }
    }
}