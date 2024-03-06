using StateSyncServer.LogicScripts.VirtualClient.Bases;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Scene
{
    public class RegSceneClass : RegisterBase<RegSceneClass, string, Type>
    {
        public static string GameRoomScene = "GameRoom";

        public override void Init()
        {
            base.Init();
            regDic.Add(GameRoomScene, typeof(GameRoomScene));
        }
    }
}