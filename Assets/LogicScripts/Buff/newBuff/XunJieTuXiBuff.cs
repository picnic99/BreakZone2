using UnityEngine;
/// <summary>
/// 迅捷突袭 10s后下次造成伤害额外附加100点伤害 以及30%的减速效果 触发后持续5s
/// </summary>
public class XunJieTuXiBuff : BuffGroup
{
    public float duration_time = 10f;
    public XunJieTuXiBuff()
    {
        buffData = new BuffVO("迅捷突袭",10f);
        durationTime = buffData.durationTime;
    }

    public override void Init()
    {
        base.Init();
        TriggerBuffComponent buff = AddTriggerBuff();
        buff.AddTrigger_DURATION_TIME(buffData.durationTime);
        buff.AddEnd_DURATION_TIME(buffData.durationTime);
        buff.doBuffCall = (args) =>
        {
            float trigger_duration_time = buffData.durationTime / 2;
            durationTime = trigger_duration_time;
            buff.AddEnd_DURATION_TIME(durationTime);
            buff.askCall = (action) =>
            {
                if (action.from.character == character && action is DamageAction)
                {
                    action.value.AddExAddValue(100);//数值可提取
                    AddBuff(action.targets, new BuffVO("迅捷突袭减速", trigger_duration_time), (buff) =>
                    {
                        buff.AddBuffComponent(buff.AddPropertyBuff(trigger_duration_time, new PropertyBuffVO(PropertyType.MOVESPEED, false, -0.3f)));//数值可提取
                    });
                    Debug.Log("[迅捷突袭] 触发成功 附加100点伤害以及30%的减速效果！");
                    buff.AddEnd_TRIGGER_COUNT(buff.curTriggerCtn + 1);
                    buff.curTriggerCtn++;
                }
            };
        };
        AddBuffComponent(buff);
    }

    public override BuffFlag GetFlag()
    {
        return BuffFlag.NAGATIVE;
    }

    public override string GetDesc()
    {
        return "[迅捷突袭]:10s后下次造成伤害额外附加100点伤害 以及30%的减速效果 持续时间" + durationTime.ToString("F2") + "s";
    }
}