//审判技能
using System;
using UnityEngine;

public class ShenPanSkill : Skill
{
    private float damageFrequency = 0f;

    public ShenPanSkill()
    {
        skillDurationTime = stateDurationTime = 0.6f;
    }

    public override void OnEnter()
    {
        character.KeepMove = true;
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        //播放审判动画
        //人物保持正常移动
        //character.scan.ShowRange(360, 2f, 1f);
        //EffectManager.GetInstance().PlayEffect("Skill/ShenPan", 0.7f, null, character.trans.position, character.trans.forward, new Vector3(0.15f, 0.2f, 0.15f));
        new ShenPanInstance(character, BeTrigger);
    }

    /// <summary>
    /// 强制中断处理
    /// </summary>
    public override void ForceStop()
    {
        base.ForceStop();
    }

    /// <summary>
    /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
    /// </summary>
    /// <param name="target"></param>
    public void BeTrigger(Character[] target)
    {
        float damageValue = 50;// baseDamage[skillLevel] + 0.5f * character.property.atk;
        DoDamage(target, damageValue);
        foreach (var item in target)
        {
            var dir = (item.trans.position - character.trans.position).normalized;
            dir.y = 0;
            item.physic.Move(dir.normalized * 0.2f, 0.1f);
        }
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
    Action<Character[]> call;
    string path = "Skill/ShenPan";
    float skillDurationTime = 0.6f;
    public ShenPanInstance(Character character, Action<Character[]> call)
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
        skillInstance.transform.localScale = new Vector3(0.15f, 0.2f, 0.15f);
        skillInstance.transform.SetParent(character.trans);
    }

    private void AddBehaviour()
    {
        skillInstance.GetComponent<ColliderHelper>().OnTriggerStayCall += DoTrigger;
        TimeManager.GetInstance().AddFrameLoopTimer(this, 0f ,0.61f, () =>
        {
            if (skillDurationTime % 0.2f == 0)
            {
                canTrigger = true;
            }
            skillDurationTime -= 0.02f;
        },()=>
        {
            TimeManager.GetInstance().RemoveAllTimer(this);
            GameObject.Destroy(skillInstance);
        });
    }

    bool canTrigger = false;
    private void DoTrigger(Collider col)
    {
        if (canTrigger == false) return;
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null || target == character) return;

        call.Invoke(new Character[] { target });
        canTrigger = false;
    }
}