
using UnityEngine;

/// <summary>
/// 技能的基类
/// 所以技能必须继承基类实现
/// 目前有盖伦的四个技能示例
///       OnUpdate
/// OnEnter > OnTrigger > OnBack> OnEnd > OnExit
/// 
/// 造成伤害会赋予对应的受击动画
/// 一个技能可能会造成多端伤害 每段伤害都应该支持配置不同的受击动画
/// 
/// </summary>
public abstract class Skill : Behaviour
{
    private EventDispatcher eventDispatcher;
    public EventDispatcher EventDispatcher {
        get
        {
            if (eventDispatcher == null) eventDispatcher = new EventDispatcher();
            return eventDispatcher;
        }
    }
    //技能数据 保存必要信息 支持配置
    public SkillVO skillData;
    //技能持续时间
    public float skillDurationTime;
    //状态持续时间
    public float stateDurationTime;
    //是否是脱手技能
    //public bool isOutHandSkill = false;
    //施法结束了吗？
    protected bool releaseOver = false;
    //技能完毕后移除生成的Buff
    protected bool IsExitRemoveBuff = false;
    //技能能否再次触发
    public bool CanTriggerAgain = false;
    //当前技能阶段
    public int StageNum = 0;
    /// <summary>
    /// 所属状态 此技能为状态技能
    /// </summary>
    public string belongState;
    //当前技能所处状态 前摇 施放 后摇
    public SkillInStateEnum skillState = SkillInStateEnum.FRONT;

    public float curAnimLength = 0;
    public Character Character
    {
        get { return character; }
        set
        {
            character = value;
            character.AddSkillBehaviour(this);
        }
    }

    /// <summary>
    /// 设置技能施放结束
    /// 用处：此处可以
    /// </summary>
    protected virtual void SetReleaseOver()
    {
        releaseOver = true;
    }

    /// <summary>
    /// 技能状态结束时 通知一下移除状态
    /// 两种情况需要调用此函数 
    /// 1.技能被打断的情况 现在大部分技能是能够预测状态退出时间的也就是技能时长结束 状态机那边会自动移除状态
    /// 出现被打断的情况 时需要通知状态机
    /// 2.无法预测退出时间的技能，比如跳跃之类的 需要满足退出条件了通知状态机
    /// </summary>
    protected virtual void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.DoSkill);
    }

    /// <summary>
    /// 技能是否是否完成？
    /// 目前由两个条件决定 一个是进入到后摇状态另一个是技能时间已到
    /// </summary>
    /// <returns></returns>
    public bool IsSkillReleaseOver()
    {
        return skillState == SkillInStateEnum.BACK || releaseOver;
    }

    public override void OnEnter()
    {
        // 瞬时技能
        // 创建完毕立马施放并退出技能状态
        if (skillData.IsInstantSkill)
        {
            //直接触发技能逻辑
            OnTrigger();
            //立刻进入后摇
            OnBack();
            //抛出事件移除技能状态
            EndState();
            return;
        }

        var cur_animName = skillData.GetAnimKeyBySkillIndex(StageNum);
        float animTime = AnimManager.GetInstance().GetAnimTime2(cur_animName);
        var animCfg = AnimConfiger.GetInstance().GetAnimByAnimKey(cur_animName);
        animTime = animTime * animCfg.animation.ValidLength;
        curAnimLength = animTime;
        if (animTime == 0) animTime = stateDurationTime;
        //前摇时间 此时一般在播放抬手动画 抬手动画可以通过
        //暂时先不考虑抬手动作吧 理由：没有合适的取消施放按键
        //OnFront();   

        Debug.Log($"当前技能为：{skillData.skill.Name},前摇：{skillData.GetFrontTime(StageNum)},后摇：{skillData.GetBackTime(StageNum)}");
        //技能触发 已经算在施放技能了
        TimeManager.GetInstance().AddOnceTimer(this, animTime * skillData.GetFrontTime(StageNum), () =>
        {
            OnTrigger();
        });

        //后摇时间 处于后摇时间的话可以通过其它技能打断当前收尾动画 快速施放下一个动画 TODO：控制哪些技能可以打断收尾动画
        TimeManager.GetInstance().AddOnceTimer(this, animTime * skillData.GetBackTime(StageNum), () =>
        {
            OnBack();
        });
        
        //默认状态时间等于当前阶段的动画时间
        stateDurationTime = curAnimLength;
        //默认技能时间等于状态持续时间
        skillDurationTime = stateDurationTime;
    }

    public override void OnUpdate()
    {
        skillDurationTime -= Time.deltaTime;
        Debug.Log($"当前技能为：{skillData.skill.Name},剩余时间：{skillDurationTime}");
        if (skillDurationTime <= 0)
        {
            OnEnd();
        }
    }

    /// <summary>
    /// 技能被打断时的处理
    /// </summary>
    public virtual void ForceStop()
    {
        OnEnd();
    }

    protected override void OnEnd()
    {

        SetReleaseOver();
        //技能结束时 移除掉角色的引用
        EndState();
        character.RemoveSkillBehaviour(this);
    }

    /// <summary>
    /// 技能被移除
    /// </summary>
    public override void OnExit()
    {
        if (!releaseOver)
        {
            SetReleaseOver();
        }
        if (IsExitRemoveBuff)
        {
            BuffManager.GetInstance().RemoveAllBuffFromSkill(character, this);
        }
    }

    /// <summary>
    /// 触发
    /// </summary>
    public virtual void OnTrigger()
    {
        skillState = SkillInStateEnum.TRIGGER;
        StageNum++;
    }
    /// <summary>
    /// 后摇
    /// </summary>
    public virtual void OnBack()
    {
        skillState = SkillInStateEnum.BACK;
    }

    public override string GetDesc()
    {
        return $"[{skillData.SkillName}]当前处于{skillState}状态 剩余时间{skillDurationTime};";
    }
}