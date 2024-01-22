using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegUIClass : RegisterBase<RegUIClass, string, Type>
{
    public static string SelectRoleUI = "SelectRoleUI";

    public override void Init()
    {
        base.Init();
        regDic.Add(RegUIClass.SelectRoleUI, typeof(SelectRoleUI));
    }

    public Type GetUIType(string key)
    {
        if (regDic.ContainsKey(key))
        {
            return regDic[key];
        }

        return null;
    }
}
