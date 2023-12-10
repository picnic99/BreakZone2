using System;
using UnityEngine;

public class FaShuYongDongSkill : Skill
{

    public FaShuYongDongSkill()
    {
        skillDurationTime = stateDurationTime = 0.7f;
    }

    public override void OnEnter()
    {
        PlayAnim("atk2");
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        new FaShuYongDong(character, BeTrigger);
    }

    /// <summary>
    /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(Character[] target)
    {
        AddBuff(target, new BuffVO("法术涌动标记", 5f), (buff) =>
        {
            buff.AddBuffComponent(buff.AddTagBuff(TagType.FASHUYONGDONG, 5f));
        });

        DoDamage(target, 60);
    }

    class FaShuYongDong
    {
        Character character;
        GameObject skillInstance;
        Action<Character[]> call;
        string path = "Skill/FaShuYongDong";
        float skillDurationTime = 5f;
        public FaShuYongDong(Character character, Action<Character[]> call)
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
            skillInstance.transform.position = character.trans.position + character.trans.forward + new Vector3(0, 1, 0);
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
                skillInstance.transform.position += skillInstance.transform.forward * Time.deltaTime * 10f;
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

            skillDurationTime = 0;
            triggered = true;
        }
    }
}