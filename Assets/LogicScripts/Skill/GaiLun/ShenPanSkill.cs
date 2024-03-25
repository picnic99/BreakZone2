using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 审判 temp
/// 角色挥动武器向前旋转 召唤龙卷风向前飞去
/// 对命中的敌人造成伤害和击飞效果
/// </summary>
public class ShenPanSkill : Skill
{
    public ShenPanSkill()
    {

    }

    public override void OnEnter()
    {
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
        skillDurationTime = stateDurationTime = 0.6f;
        AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "start", this.character.trans.gameObject);

    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        new ShenPanInstance(this, BeTrigger);
    }

    /// <summary>
    /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(_Character target)
    {
        float damageValue = 50;// baseDamage[skillLevel] + 0.5f * character.property.atk;
        DoDamage(target, damageValue);
        target.physic.AtkFly(5f, 1f);
    }

    protected override void OnEnd()
    {
        character.KeepMove = false;
        base.OnEnd();
    }

    public override string GetDesc()
    {
        return "当前技能为：审判，剩余时间为：" + skillDurationTime.ToString("F2");
    }
}

class ShenPanInstance : SkillInstance
{
    float skillDurationTime1 = 0.4f;
    float skillDurationTime2 = 0.6f;

    List<_Character> triggeredTargets = new List<_Character>();
    public ShenPanInstance(Skill skill, Action<_Character> call)
    {
        this.RootSkill = skill;
        this.enterCall = call;
        this.instancePath = "ShenPan2";
        this.instanceObj = ResourceManager.GetInstance().GetSkillInstance(instancePath);
        this.Init();
    }

    public override void InitTransform()
    {
        var crt = RootSkill.character;
        instanceObj.transform.forward = crt.trans.forward;
        instanceObj.transform.position = crt.trans.position;
        instanceObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); //shenpan2
        instanceObj.transform.SetParent(crt.trans);
    }

    public override void AddBehaviour()
    {
        var crt = RootSkill.character;
        crt.physic.Move(crt.trans.forward.normalized * 2.5f, 0.3f);
        TimeManager.GetInstance().AddFrameLoopTimer(this, 0f, skillDurationTime1 + skillDurationTime2, () =>
        {
            if (skillDurationTime1 > 0)
            {
                skillDurationTime1 -= Time.deltaTime;
            }
            else if (skillDurationTime2 > 0)
            {
                instanceObj.transform.SetParent(null);
                instanceObj.transform.position += instanceObj.transform.forward * Time.deltaTime * 20f;
            }
        },
        End);
    }

    public override void InvokeEnterTrigger(_Character target)
    {
        var crt = RootSkill.character;
        if (target == null || target == crt) return;
        if (triggeredTargets.IndexOf(target) != -1)
        {
            return;
        }
        triggeredTargets.Add(target);
        base.InvokeEnterTrigger(target);
    }
}