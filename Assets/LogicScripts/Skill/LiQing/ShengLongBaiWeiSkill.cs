using DG.Tweening;
using System;
using UnityEngine;

public class ShengLongBaiWeiSkill : Skill
{

    public ShengLongBaiWeiSkill()
    {
    }

    public override void OnEnter()
    {
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
        skillDurationTime = stateDurationTime = 1f;
        Time.timeScale = 0.4f;
        CameraManager.GetInstance().ShowFeatureCam(character);
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        new ShengLongBaiWei(this, BeTrigger);
        CameraManager.GetInstance().ShowMainCam(character);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(Character[] target)
    {
        DoDamage(target, 150);
        foreach (var item in target)
        {
            item.physic.Move(character.trans.forward.normalized * 5f, 0.25f);
        }
    }

    class ShengLongBaiWei : SkillInstance
    {
        Action<Character[]> call;
        public ShengLongBaiWei(Skill skill, Action<Character[]> call)
        {
            this.RootSkill = skill;
            this.instancePath = "Skill/ShengLongBaiWei";
            this.durationTime = 2f;
            this.maxTriggerTarget = 99;
            this.IsEndRemoveObj = false;
            this.call = call;
            this.instanceObj = ResourceManager.GetInstance().GetObjInstance<GameObject>(instancePath);

            this.Init();
        }

        public override void InvokeEnterTrigger(Character target)
        {
            call.Invoke(new Character[] { target });
        }

        public override void InitTransform()
        {
            instanceObj.transform.forward = RootSkill.character.trans.forward;
            instanceObj.transform.position = RootSkill.character.trans.position + RootSkill.character.trans.forward * 1f;
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
    }
}