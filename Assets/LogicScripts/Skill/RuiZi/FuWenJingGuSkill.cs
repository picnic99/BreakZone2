
using System;
using UnityEngine;
/// <summary>
/// 符文禁锢
/// </summary>
public class FuWenJingGuSkill : Skill
{
    public FuWenJingGuSkill()
    {
        skillDurationTime = stateDurationTime = 1f;
    }

    public override void OnEnter()
    {
        PlayAnim("atk2");
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        new FuWenJingGu(character, this);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(_Character[] target)
    {
        DoDamage(target, 60);
    }

    class FuWenJingGu
    {
        _Character character;
        GameObject skillInstance;
        Transform slowEffect;
        Transform dizzyEffect;
        Transform flyEffect;
        Skill skill;
        string path = "FuWenJingGu";
        float skillDurationTime = 5f;
        public FuWenJingGu(_Character character, Skill skill)
        {
            this.character = character;
            this.skill = skill;
            skillInstance = ResourceManager.GetInstance().GetSkillInstance(path);
            this.InitTransform();
            this.AddBehaviour();
        }

        private void InitTransform()
        {
            dizzyEffect = skillInstance.transform.Find("dizzyEffect");
            slowEffect = skillInstance.transform.Find("slowEffect");
            flyEffect = skillInstance.transform.Find("flyEffect");
            skillInstance.transform.forward = character.trans.forward;
            skillInstance.transform.position = character.trans.position + character.trans.forward;
        }

        private void AddBehaviour()
        {
            flyEffect.GetComponent<ColliderHelper>().OnTriggerEnterCall += DoTrigger;
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
            var character = GameContext.GetCharacterByObj(col.gameObject);
            if (character == null) return;
            skill.DoDamage(character, 60);


            TagBuffComponent tag = character.HasTagBuff(TagType.FASHUYONGDONG);
            if (tag != null)
            {
                skill.DoDamage(character, 50);
                skill.AddState(character, character, StateType.Trap, 1.5f);
                GameObject effect = GameObject.Instantiate<GameObject>(dizzyEffect.gameObject, col.transform);
                effect.SetActive(true);
                TimeManager.GetInstance().AddOnceTimer(effect, 1.5f, () =>
                {
                    GameObject.Destroy(effect);
                });
                tag.durationTime = 0;
            }
            else
            {
                skill.AddBuff(character, new BuffVO("符文禁锢减速", 3f), (buff) =>
                {
                    buff.AddBuffComponent(buff.AddPropertyBuff(3f, new PropertyBuffVO(PropertyType.MOVESPEED, false, -0.35f)));
                });
                GameObject effect = GameObject.Instantiate<GameObject>(slowEffect.gameObject, col.transform);
                effect.SetActive(true);
                TimeManager.GetInstance().AddOnceTimer(effect, 3f, () =>
                {
                    GameObject.Destroy(effect);
                });
            }

            skillDurationTime = 0;
            triggered = true;
        }
    }

}

