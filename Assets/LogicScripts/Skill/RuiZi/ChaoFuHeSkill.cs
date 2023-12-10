using System;
using UnityEngine;
/// <summary>
/// 技能：超负荷
/// 向一个方向施放一个法球，对触碰到的第一个敌方单位造成伤害
/// 同时引爆其身上的法术涌动效果
/// </summary>
public class ChaoFuHeSkill : Skill
{
    public string skillInstance = "ChaoFuHe";

    public ChaoFuHeSkill()
    {
        skillDurationTime = stateDurationTime = 0.5f;
    }

    public override void OnEnter()
    {
        PlayAnim("atk1");
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        //创建技能实体
        new ChaoFuHe(character, BeTrigger);
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
            TagBuffComponent tag = item.HasTagBuff(TagType.FASHUYONGDONG);
            if (tag != null)
            {
                Skill skill = (Skill)tag.from;
                skill.DoDamage(target, 150);
                tag.durationTime = 0;
            }
        }
    }

    class ChaoFuHe
    {
        Character character;
        GameObject skillInstance;
        Action<Character[]> call;
        string path = "Skill/ChaoFuHe";
        float skillDurationTime = 3f;
        public ChaoFuHe(Character character,Action<Character[]> call)
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
            skillInstance.transform.position = character.trans.position + character.trans.forward + new Vector3(0,1,0);
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
                skillInstance.transform.position += skillInstance.transform.forward * Time.deltaTime * 20f;
                skillDurationTime -= 0.02f;
            });
        }

        bool triggered = false;
        private void DoTrigger(Collider col)
        {
            if (triggered) return;
            var target = GameContext.GetCharacterByObj(col.gameObject);
            if (target == null || target == character) return;

            call.Invoke( new Character[] { target });

            skillDurationTime = 0;
            triggered = true;
        }
    }

}