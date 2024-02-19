using UnityEngine;


public enum BuffFlag
{
    POSITIVE, //正面的
    NAGATIVE, //负面的
    OTHER,
}

/// <summary>
/// buff基类
/// 默认支持数值类的buff修改，无需继承
/// </summary>
public abstract class Buff : Behaviour
{
    //Buff的来源 可能来自一个skill或者是一个state 记录一下以后可能会用上
    public Behaviour from;
    //Buff的数据 存储一些基本信息 支持配置
    public BuffVO buffData;

    public bool IsEnable = true;

    //为该buff指定作用角色
    public virtual Character Character
    {
        get { return character; }
        set
        {
            character = value;
            character.AddBuffBehaviour(this);
            Debug.Log("[BUFF载入] " + character.characterData.characterName + " -- " + buffData.buffName);
        }
    }

    public override void OnEnter()
    {
        Debug.Log("[BUFF执行] " + character.characterData.characterName + " -- " + buffData.buffName);

    }


    protected override void OnEnd()
    {
        //从角色身上移除掉该BUFF的引用
        character.RemoveBuffBehaviour(this);
        IsEnable = false;
        Debug.Log("[BUFF结束] " + character.characterData.characterName + " -- " + buffData.buffName);
    }

    public void OnDestroy()
    {
        OnEnd();
    }

    public override void OnExit()
    {
        Debug.Log("[BUFF卸载] " + character.characterData.characterName + " -- " + buffData.buffName);
    }

    /// <summary>
    /// 获取该BUFF是否正面还是负面
    /// </summary>
    /// <returns></returns>
    public abstract BuffFlag GetFlag();
}