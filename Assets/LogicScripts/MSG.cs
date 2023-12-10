using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSG
{
    public string characterName;
    public string curSkillName;
    public string characterProperty;
    public string curState;
    public string curBuff;
    public string curSkill;
    public string desc;

    public string ShowMsg()
    {
        return "角色名称：" + characterName + "\n"
            + "角色数值：" + characterProperty + "\n"
            + "当前状态：" + curState + "\n"
            + "当前挂载的BUFF：" + curBuff + "\n"
            + "当前挂载的技能：" + curSkill + "\n"
            + "描述：" + desc + "\n";

    }

    public string ShowSimpleMsg()
    {

        string msg = "property：" + characterProperty + "\n"
                    + "state：" + curState + "\n"
                    + "buff：" + curBuff + "\n"
                    + "skill：" + curSkill + "\n";
        return msg;
    }
}
