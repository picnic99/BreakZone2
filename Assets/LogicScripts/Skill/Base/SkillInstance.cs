using System;
using UnityEngine;

/// <summary>
/// 技能实例 由Skill类创建
/// 目的 协助技能的行为 如创建特效 检测目标
/// 暂时想法是 skill 负责调度 和决定最终结果 
/// 如何时创建什么实例 以及实例检测目标后执行什么逻辑 
/// 击飞 击退 造成伤害恢复血量施加buff等
/// </summary>
public abstract class SkillInstance
{
    /// <summary>
    /// 所属技能
    /// </summary>
    public Skill RootSkill;
    /// <summary>
    /// 实例object
    /// </summary>
    public GameObject instanceObj;
    /// <summary>
    /// 实例的路径
    /// </summary>
    public string instancePath = "";
    /// <summary>
    /// 实例持续时间
    /// </summary>
    public float durationTime = 1f;
    /// <summary>
    /// 实例的回调 可能有多个 如触碰目标的回调 如实例结束的回调 如实例开始的回调 。。。
    /// </summary>
    public Action<object[]> enterCall;
    public Action<object[]> stayCall;
    public Action<object[]> exitCall;
    /// <summary>
    /// 实例的范围检测碰撞器 也可能有多个
    /// </summary>
    public ColliderHelper collider;
    /// <summary>
    /// 角色死亡时技能移除
    /// </summary>
    public bool IsCharacterDeadRemove;
    /// <summary>
    /// 技能是否结束
    /// </summary>
    public bool IsEnd = false;
    /// <summary>
    /// 最大触发目标数
    /// </summary>
    public int maxTriggerTarget = 1;
    /// <summary>
    /// 当前触发目标数
    /// </summary>
    protected int curTriggerTarget = 0;
    /// <summary>
    /// 是否需要碰撞器检测
    /// </summary>
    protected bool needTriggerCheck = true;

    public virtual void Init(string layerName = "Character", CharacterState TriggerType = CharacterState.ENEMY)
    {
        InitTransform();
        AddBehaviour();
        if (needTriggerCheck)
        {
            //碰撞盒
            collider = this.instanceObj.GetComponent<ColliderHelper>();
            SetTriggerInfo(layerName, TriggerType);
            collider.OnTriggerEnterCall += OnEnterTrigger;
            collider.OnTriggerStayCall += OnStayTrigger;
            collider.OnTriggerExitCall += OnExitTrigger;
        }
    }

    public virtual void SetTriggerInfo(string layerName, CharacterState TriggerType)
    {
        var layer = LayerMask.NameToLayer(layerName);
        //设置检测的层以及目标
        collider.SetInfo(layer, TriggerType);
    }

    /// <summary>
    /// 初始化实例创建的位置信息
    /// </summary>
    public abstract void InitTransform();
    /// <summary>
    /// 添加实例的行为 如移动 炸裂 旋转 等等
    /// </summary>
    public abstract void AddBehaviour();
    /// <summary>
    /// 触发时逻辑
    /// </summary>
    public virtual void OnEnterTrigger(Collider col)
    {
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null) return;
        if (target.state != collider.info.TriggerType)
        {
            return;
        }
        curTriggerTarget++;

        InvokeEnterTrigger(target);

        if (curTriggerTarget >= maxTriggerTarget)
        {
            End();
        }
    }
    public virtual void OnStayTrigger(Collider col)
    {

    }
    public virtual void OnExitTrigger(Collider col)
    {

    }

    public virtual void InvokeEnterTrigger(Character target) { }
    public virtual void InvokeStayTrigger(Character target) { }
    public virtual void InvokeExitTrigger(Character target) { }

    public virtual void End()
    {
        IsEnd = true;
        durationTime = 0;
        if (needTriggerCheck)
        {
            collider.OnTriggerEnterCall -= OnEnterTrigger;
            collider.OnTriggerStayCall -= OnStayTrigger;
            collider.OnTriggerExitCall -= OnExitTrigger;
        }
        TimeManager.GetInstance().RemoveAllTimer(this);
        GameObject.Destroy(instanceObj);
    }
}