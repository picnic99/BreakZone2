public abstract class BaseAction
{
    //行为发出者
    public _Character owner;
    //行为接收者
    public _Character[] targets;
    //数值
    public PropertyValue value;
    //来源
    public Behaviour from;
    //来源描述
    public string fromStr;

    public void Init(_Character character, _Character[] targets, float value, Behaviour from)
    {
        this.owner = character;
        this.targets = targets;
        this.value = new PropertyValue(value);
        this.from = from;
    }
    public abstract void Apply();

    //下次普通攻击伤害提升100% buff
    //下次受到普攻的伤害】、

    //考虑属性 buff

    public bool IsFromSkill()
    {
        return from is Skill;
    }

    public bool IsFromBuff()
    {
        return from is Buff;
    }
}