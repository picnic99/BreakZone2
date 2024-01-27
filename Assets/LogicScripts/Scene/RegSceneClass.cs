using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegSceneClass :RegisterBase<RegSceneClass, string, Type>
{
    public static string SelectRoleScene = "SelectRole";
    public static string GameRoomScene = "GameRoom";

    public override void Init()
    {
        base.Init();
        regDic.Add(RegSceneClass.SelectRoleScene, typeof(SelectRoleScene));
        regDic.Add(RegSceneClass.GameRoomScene, typeof(GameRoomScene));
    }
}
