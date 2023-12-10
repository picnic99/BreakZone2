using System.Collections.Generic;

public abstract class BuffComponent : Buff
{
    public BuffGroup group;
    public EventDispatcher eventDispatcher = new EventDispatcher();
    /// <summary>
    /// buff标签
    /// </summary>
    public List<int> tags = new List<int>();

    //为该buff指定作用角色
    public override Character Character
    {
        get { return character; }
        set
        {
            character = value;
        }
    }

    public override void OnEnter()
    {

    }

    public virtual void BuffAsk(BaseAction action)
    {

    }


    protected override void OnEnd()
    {
        if (group != null)
        {
            group.RemoveBuffComponent(this);
        }
    }

    public override void OnExit()
    {

    }
}