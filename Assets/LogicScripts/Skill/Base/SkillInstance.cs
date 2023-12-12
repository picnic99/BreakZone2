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
    float durationTime = 1f;
    /// <summary>
    /// 实例的回调 可能有多个 如触碰目标的回调 如实例结束的回调 如实例开始的回调 。。。
    /// </summary>
    Action<object[]> call;
    /// <summary>
    /// 实例的范围检测碰撞器 也可能有多个
    /// </summary>
    ColliderHelper collider;

    /// <summary>
    /// 初始化实例创建的位置信息
    /// </summary>
    public abstract void InitTransform();
    /// <summary>
    /// 添加实例的行为 如移动 炸裂 旋转 等等
    /// </summary>
    public abstract void AddBehaviour();
}