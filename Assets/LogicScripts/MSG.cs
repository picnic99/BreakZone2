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
        return "��ɫ���ƣ�" + characterName + "\n"
            + "��ɫ��ֵ��" + characterProperty + "\n"
            + "��ǰ״̬��" + curState + "\n"
            + "��ǰ���ص�BUFF��" + curBuff + "\n"
            + "��ǰ���صļ��ܣ�" + curSkill + "\n"
            + "������" + desc + "\n";

    }

    public string ShowSimpleMsg()
    {

        string msg = "property��" + characterProperty + "\n"
                    + "state��" + curState + "\n"
                    + "buff��" + curBuff + "\n"
                    + "skill��" + curSkill + "\n";
        return msg;
    }
}
