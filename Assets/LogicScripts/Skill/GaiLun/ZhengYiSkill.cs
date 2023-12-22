using System;
using UnityEngine;

/// <summary>
/// 正义(暂时)
/// 玩家向前跃起召唤剑阵，击飞剑阵内的敌人后，召唤剑舞，对剑阵内的敌人造成伤害同时向中心聚拢
/// 技能持续时间 5秒  触发后击飞剑阵范围内敌人 伤害开始时间 1s后 持续3秒 共计9段伤害
/// 状态持续时间 2秒
/// 主要流程：召唤剑阵 击飞敌人 开始剑舞 造成伤害以及向中间点位移
/// </summary>
public class ZhengYiSkill : Skill
{
    public ZhengYiSkill()
    {
        //退出时移除施加的buff
        this.IsExitRemoveBuff = true;
    }
    public override void OnEnter()
    {
        //播放正义动画
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
        stateDurationTime = skillDurationTime = 2f;
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        new ZhenYiInstance(this, BeTrigger);
    }

    public void BeTrigger(Character target)
    {
        DoDamage(target, 25);
        var dir = (target.trans.position - character.trans.position).normalized;
        dir.y = 2f;
        target.physic.Move(dir.normalized * 0.2f, 0.1f);
    }

    /*    protected override void OnEnd()
        {
            //移除掉来自技能施加的所有buff效果
            BuffManager.GetInstance().RemoveAllBuffFromSkill(character, this);
            SetReleaseOver();
            base.OnEnd();
        }*/
}

class ZhenYiInstance : SkillInstance
{
    private float atkFlyTimePoint = 1f; //击飞时间点
    private float startDamageTimePoint = 2f; //伤害开始时间点
    private float damageCD = 0.3f;//伤害间隔
    private int maxDamageCtn = 9;//最大伤害次数

    private ColliderHelper atkFlyCheck;
    private ColliderHelper JianWuCheck;

    public ZhenYiInstance(Skill skill, Action<Character> call)
    {
        this.RootSkill = skill;
        this.enterCall = call;
        this.instancePath = "Skill/ZhenYi";
        this.durationTime = 5f;
        this.instanceObj = ResourceManager.GetInstance().GetObjInstance<GameObject>(instancePath);
        this.atkFlyCheck = this.instanceObj.transform.Find("atkFlyCheck").GetComponent<ColliderHelper>();
        this.JianWuCheck = this.instanceObj.transform.Find("JianWuCheck").GetComponent<ColliderHelper>();
        this.enterCall = call;
        this.Init();
    }

    public override void AddBehaviour()
    {
        TimeManager.GetInstance().AddOnceTimer(this,0.2f,()=>
        {
            atkFlyCheck.gameObject.SetActive(true);
            atkFlyCheck.OnTriggerEnterCall += AtkFly;
        });
        TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
        {
            atkFlyCheck.OnTriggerEnterCall -= AtkFly;
            atkFlyCheck.gameObject.SetActive(false);
        });

        float totalTime = 3f;
        float cd = totalTime / maxDamageCtn;
        JianWuCheck.OnTriggerEnterCall += FeiWuAtk;
        //剑舞
/*        for (int i = 0; i < maxDamageCtn; i++)
        {
            var triggerTime = 1f+i * cd;
            Debug.Log("第" + i + "次剑舞攻击" + "攻击时间" + triggerTime + "结束时间" + triggerTime + Time.deltaTime);
            TimeManager.GetInstance().AddOnceTimer(this, triggerTime, () =>
            {
                JianWuCheck.gameObject.SetActive(true);
            });
            TimeManager.GetInstance().AddOnceTimer(this, triggerTime + 0.02f, () =>
            {
               JianWuCheck.gameObject.SetActive(false);
            });
        }*/

        TimeManager.GetInstance().AddTimeByDurationCtn(this, totalTime, maxDamageCtn, () =>
         {
             JianWuCheck.gameObject.SetActive(true);
             TimeManager.GetInstance().AddFixedFrameCall(() =>
             {
                 JianWuCheck.gameObject.SetActive(false);
             },3);
         });

        //击飞
        TimeManager.GetInstance().AddLoopTimer(this, 0, () =>
                {
                    if (durationTime <= 0)
                    {
                        End();
                        return;
                    }
                    durationTime -= Time.deltaTime;
                });
    }

    public void AtkFly(Collider col)
    {
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null || target == RootSkill.character) return;
        target.physic.AtkFly(1, 1f);
    }

    public void FeiWuAtk(Collider col)
    {
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null || target == RootSkill.character) return;
        target.physic.Move((this.instanceObj.transform.position - target.trans.position) * 0.3f, 0.5f);
        enterCall.Invoke(target);
    }

    public override void InitTransform()
    {
        instanceObj.transform.position = RootSkill.character.trans.position + RootSkill.character.trans.forward * 7f;
        instanceObj.transform.forward = RootSkill.character.trans.forward;
    }


}