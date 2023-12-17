using System;
using UnityEngine;

public class GaiLunAttack : BaseAttack
{
    public string effectPath = "";

    public GaiLunAttack()
    {
        CanTriggerAgain = true;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        int index = StageNum + 1;
        TimeManager.GetInstance().AddOnceTimer(this, skillData.GetFrontTime(StageNum), () =>
          {
            new GaiLunAtkInstance(this, index, DoDamage);
          });
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
    }

    public void DoDamage(Character target)
    {
        AddState(target, character, StateType.Injure);
        DoDamage(target, character.property.Atk);
        //顿帧
        character.anim.speed = 0f;
        TimeManager.GetInstance().AddOnceTimer(this, 0.05f, () =>
        {
            character.anim.speed = 1;
        });
        var dir = (character.trans.forward).normalized;
        dir.y = 0;
        target.physic.Move(dir.normalized * 0.1f, 0.1f);
        character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });
    }

    protected override void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.DoAtk);
    }


    public override void OnExit()
    {
        base.OnExit();
        character.anim.speed = 1;
    }
}

class GaiLunAtkInstance : SkillInstance
{
    public GameObject curAtk;

    Action<Character> call;
    public GaiLunAtkInstance(Skill skill, int index ,Action<Character> call)
    {
        this.RootSkill = skill;
        this.instancePath = "Skill/GaiLunAtk";
        this.durationTime = 1f;
        this.maxTriggerTarget = 99;
        this.IsEndRemoveObj = false;
        this.call = call;
        this.instanceObj = ResourceManager.GetInstance().GetObjInstance<GameObject>(instancePath);
        this.instanceObj.transform.SetParent(skill.character.trans);
        this.curAtk = this.instanceObj.transform.Find("atk" + index).gameObject;
        this.curAtk.SetActive(true);
        this.Init();
    }

    public override void SetCollider(string layerName, CharacterState TriggerType)
    {
        this.collider = this.curAtk.transform.Find("collider").GetComponent<ColliderHelper>();
        SetTriggerInfo(layerName, TriggerType);
        collider.OnTriggerEnterCall += OnEnterTrigger;
    }

    public override void OnEnterTrigger(Collider col)
    {
        base.OnEnterTrigger(col);
        Vector3 v = col.ClosestPointOnBounds(RootSkill.Character.trans.position);
        var bloodEffect = ResourceManager.GetInstance().GetObjInstance<GameObject>("Common/BloodEffect");
        bloodEffect.transform.position = v;
        TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
        {
            GameObject.Destroy(bloodEffect);
        });
    }

    public override void InvokeEnterTrigger(Character target)
    {
        call.Invoke(target);
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