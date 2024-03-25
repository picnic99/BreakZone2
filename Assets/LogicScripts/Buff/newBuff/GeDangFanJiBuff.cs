using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 格挡反击 格挡下一个受到的伤害并反弹100点伤害 
/// </summary>
public class GeDangFanJiBuff : BuffGroup
{
    public GeDangFanJiBuff()
    {
        buffData = new BuffVO("格挡反击");
    }

    public override void Init()
    {
        base.Init();
        durationTime = 10f;
        TriggerBuffComponent buff = AddTriggerBuff();
        buff.AddEnd_TRIGGER_COUNT();
        buff.AddEnd_DURATION_TIME(durationTime);
        buff.askCall = (action) =>
        {
            if (action is DamageAction)
            {
                for (int i = 0; i < action.targets.Length; i++)
                {
                    var target = action.targets[i];
                    if (target == character)
                    {
                        buff.commonArgs = new object[] { action.from,action.value };
                        buff.AddTrigger_IMMEDIATE();
                        return;
                    }
                }
            }
        };
        buff.doBuffCall = (args) =>
        {
            if (args != null)
            {
                Behaviour from = (Behaviour)args[0];
                PropertyValue value = (PropertyValue)args[1];
                float damage = value.finalValue;
                value.AddExAddValue(-value.finalValue);
                DoDamage(new _Character[] { from.character }, 100);
                Debug.Log("格挡了"+ damage +"点伤害，并回敬对方100点");
            }
        };
        AddBuffComponent(buff);
    }

    public override BuffFlag GetFlag()
    {
        return BuffFlag.NAGATIVE;
    }
    public override string GetDesc()
    {
        if (buffComponents != null && buffComponents.Count >= 1)
        {
            return "[格挡反击]:格挡下一个受到的伤害并反弹100点伤害  持续时间" + buffComponents[0].durationTime.ToString("F2") + "s";
        }
        return "";
    }
}