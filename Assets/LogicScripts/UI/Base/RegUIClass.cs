using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegUIClass : RegisterBase<RegUIClass, string, Type>
{
    public static string SelectRoleUI = "SelectRoleUI";
    public static string DebugUI = "DebugUI";
    public static string BaseInfoOptUI = "BaseInfoOptUI";

    public override void Init()
    {
        base.Init();
        regDic.Add(RegUIClass.SelectRoleUI, typeof(SelectRoleUI));
        regDic.Add(RegUIClass.DebugUI, typeof(DebugUI));
        regDic.Add(RegUIClass.BaseInfoOptUI, typeof(BaseInfoOptUI));
    }
}
