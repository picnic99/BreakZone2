using DG.Tweening;
using System;
using UnityEngine;

public class ShengLongBaiWeiSkill : Skill
{

    public ShengLongBaiWeiSkill()
    {
        skillDurationTime = stateDurationTime = 1f;
    }

    public override void OnEnter()
    {
        PlayAnim("SHENGLONGBAIWEI");
        base.OnEnter();
        Time.timeScale = 0.4f;
        CameraManager.GetInstance().ShowFeatureCam(character);
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        new ShengLongBaiWei(character,BeTrigger);
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
            //Vector3 dir = (item.trans.position - character.trans.position).normalized;
            //TweenManager.GetInstance().MoveTo(item.trans, item.trans.position + offest * 10f, 1f);

            item.physic.Move(character.trans.forward.normalized * 5f, 0.25f);
        }
    }

    class ShengLongBaiWei
    {
        Character character;
        GameObject skillInstance;
        Action<Character[]> call;
        string path = "Skill/ShengLongBaiWei";
        float skillDurationTime = 2f;
        public ShengLongBaiWei(Character character, Action<Character[]> call)
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
            skillInstance.transform.position = character.trans.position + character.trans.forward*1f;
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

       // bool triggered = false;
        private void DoTrigger(Collider col)
        {
            //if (triggered) return;
            var target = GameContext.GetCharacterByObj(col.gameObject);
            if (target == null || target == character) return;

            call.Invoke(new Character[] { target });

            //triggered = true;
        }
    }
}