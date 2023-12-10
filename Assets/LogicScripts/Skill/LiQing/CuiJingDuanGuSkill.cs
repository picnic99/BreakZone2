using DG.Tweening;
using System;
using UnityEngine;

public class CuiJingDuanGuSkill : Skill
{

    public CuiJingDuanGuSkill()
    {
        skillDurationTime = stateDurationTime = 1f;
    }

    public override void OnEnter()
    {
        PlayAnim("CUIJINGDUANGU");
        base.OnEnter();
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        //TweenManager.GetInstance().MoveTo(character.trans, character.trans.position + character.trans.forward * 10f, durationTime);
        new CuiJingDuanGu(character, BeTrigger);
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

    class CuiJingDuanGu
    {
        Character character;
        GameObject skillInstance;
        Action<Character[]> call;
        string path = "Skill/CuiJingDuanGu";
        float skillDurationTime = 2f;
        public CuiJingDuanGu(Character character, Action<Character[]> call)
        {
            this.character = character;
            this.call = call;
            skillInstance = ResourceManager.GetInstance().GetObjInstance<GameObject>(path);
            this.InitTransform();
            this.AddBehaviour();
        }

        private void InitTransform()
        {
            skillInstance.transform.forward = character.trans.forward;
            skillInstance.transform.position = character.trans.position;
        }

        private void AddBehaviour()
        {
            skillInstance.GetComponent<ColliderHelper>().OnTriggerEnterCall += DoTrigger;
            TimeManager.GetInstance().AddLoopTimer(this, 0.02f, () =>
            {
                if (skillDurationTime <= 0)
                {
                    TimeManager.GetInstance().RemoveAllTimer(this);
                    GameObject.Destroy(skillInstance);
                    return;
                }
                skillDurationTime -= 0.02f;
            });
        }

        bool triggered = false;
        private void DoTrigger(Collider col)
        {
            if (triggered) return;
            var target = GameContext.GetCharacterByObj(col.gameObject);
            if (target == null || target == character) return;

            call.Invoke(new Character[] { target });

            triggered = true;
        }
    }

}