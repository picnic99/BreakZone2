using DG.Tweening;
using System;
using UnityEngine;

public class CuiJingDuanGuSkill : Skill
{

    public CuiJingDuanGuSkill(){}

    public override void OnEnter()
    {
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        new CuiJingDuanGu(this, BeTrigger);
        CameraManager.GetInstance().EventImpulse();
    }

    /// <summary>
    /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(Character[] target)
    {
        DoDamage(target, 60);
        foreach (var item in target)
        {
            //item.fsm.AddState(StateType.AtkFly);
            item.physic.Move(Vector3.up * 1f, 0.2f);
        }
    }

    class CuiJingDuanGu : SkillInstance
    {
        Action<Character[]> call;

        public CuiJingDuanGu(Skill skill,Action<Character[]> call)
        {
            this.instancePath = "Skill/CuiJingDuanGu";
            this.durationTime = 2f;
            this.maxTriggerTarget = 1;
            this.RootSkill = skill;
            this.IsEndRemoveObj = false;
            this.call = call;
            this.instanceObj = ResourceManager.GetInstance().GetObjInstance<GameObject>(instancePath);
            
            this.Init();
        }

        public override void InitTransform()
        {
            instanceObj.transform.forward = RootSkill.character.trans.forward;
            instanceObj.transform.position = RootSkill.character.trans.position;
        }

        public override void AddBehaviour()
        {
            TimeManager.GetInstance().AddLoopTimer(this, 0.02f, () =>
            {
                if (durationTime <= 0)
                {
                    End();
                    return;
                }
                durationTime -= 0.02f;
            });
        }

        public override void InvokeEnterTrigger(Character target)
        {
            call.Invoke(new Character[] { target });
        }
    }

}