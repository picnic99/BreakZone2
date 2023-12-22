//审判技能
using System;
using UnityEngine;

public class ShenPanSkill : Skill
{
    private float damageFrequency = 0f;

    public ShenPanSkill()
    {

    }

    public override void OnEnter()
    {
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
        skillDurationTime = stateDurationTime = 0.6f;
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
       new ShenPanInstance(character, BeTrigger);
    }

    /// <summary>
    /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(Character target)
    {
        float damageValue = 50;// baseDamage[skillLevel] + 0.5f * character.property.atk;
        DoDamage(target, damageValue);
        target.physic.Move(Vector3.up * 0.5f, 1f);
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

class ShenPanInstance
{
    Character character;
    GameObject skillInstance;
    Action<Character> call;
    string path = "Skill/ShenPan2";
    float skillDurationTime1 = 0.4f;
    float skillDurationTime2 = 0.6f;
    public ShenPanInstance(Character character, Action<Character> call)
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
        //skillInstance.transform.localScale = new Vector3(0.15f, 0.2f, 0.15f); //shenpan1
        skillInstance.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); //shenpan2
        skillInstance.transform.SetParent(character.trans);
    }

    private void AddBehaviour()
    {
        character.physic.Move(character.trans.forward.normalized * 2.5f, 0.3f);
        skillInstance.GetComponent<ColliderHelper>().OnTriggerStayCall += DoTrigger;
        float triggerCD = 0.1f;
        TimeManager.GetInstance().AddFrameLoopTimer(this, 0f ,skillDurationTime1 + skillDurationTime2, () =>
        {
            if (skillDurationTime1 > 0)
            {
                skillDurationTime1 -= Time.deltaTime;
            }else if(skillDurationTime2 > 0)
            {
                skillInstance.transform.SetParent(null);
                SelfMove();
            }
            if (triggerCD <= 0)
            {
                canTrigger = true;
                triggerCD = 0.2f;
            }
            triggerCD -= Time.deltaTime;
        },()=>
        {
            TimeManager.GetInstance().RemoveAllTimer(this);
            GameObject.Destroy(skillInstance);
        });
    }

    private void SelfMove()
    {
        this.skillInstance.transform.position += this.skillInstance.transform.forward * Time.deltaTime * 20f;
    }

    bool canTrigger = false;
    private void DoTrigger(Collider col)
    {
        //if (canTrigger == false) return;
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null || target == character) return;

        call.Invoke(target);
        canTrigger = false;
    }
}