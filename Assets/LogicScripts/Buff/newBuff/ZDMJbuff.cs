
/// <summary>
/// 震荡猛击 普攻四下 眩晕1秒 造成100点额外伤害 持续5f
/// </summary>
public class ZDMJbuff : BuffGroup
{
    public ZDMJbuff()
    {
        buffData = new BuffVO("震荡猛击");
    }

    public override void Init()
    {
        base.Init();
        durationTime = 10f;
        TriggerBuffComponent buff = AddTriggerBuff();
        buff.AddTrigger_SKILL_COUNT(SkillEnum.BASE_ATK, 4);
        buff.AddEnd_TRIGGER_COUNT();
        buff.AddEnd_DURATION_TIME(durationTime);
        buff.doBuffCall = (args) =>
        {
            AddState(new _Character[] { character }, character, StateType.Dizzy, 1.5f);
            DoDamage(new _Character[] { character }, 100);
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
            return "[震荡猛击]:普攻四下 眩晕1秒 造成100点额外伤害 持续时间" + buffComponents[0].durationTime.ToString("F2") + "s";
        }
        return "";
    }

}