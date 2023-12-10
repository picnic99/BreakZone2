using System;
using UnityEngine;

public class LiQingAttack : BaseAttack
{
    public ColliderHelper Hand_L;
    public ColliderHelper Hand_R;
    public ColliderHelper Leg_R;
    public ColliderHelper Leg_L;


    public LiQingAttack()
    {
        stateDurationTime = 0.7f;
        skillDurationTime = stateDurationTime + maxWaitTime;
        CanTriggerAgain = true;
    }

    public override void OnEnter()
    {
        Hand_L = character.anim.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<ColliderHelper>();
        Hand_R = character.anim.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<ColliderHelper>();
        Leg_L = character.anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).GetComponent<ColliderHelper>();
        Leg_R = character.anim.GetBoneTransform(HumanBodyBones.RightLowerLeg).GetComponent<ColliderHelper>();
        Hand_L.enabled = false;
        Hand_R.enabled = false;
        Leg_L.enabled = false;
        Leg_R.enabled = false;
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        skillState = SkillInStateEnum.TRIGGER;
        StageNum++;
        Atk();
    }

    public void DoDamage(Collider col)
    {
        if (triggered) return;
        var target = GameContext.GetCharacterByObj(col.gameObject);
        if (target == null || target == character) return;
        AddState(target, character, StateType.Injure);
        DoDamage(target, character.property.Atk);
        triggered = true;
        Vector3 v = character.trans.position;
        if (StageNum == 1)
        {
            v = col.ClosestPointOnBounds(Hand_L.transform.position);
        }
        else if (StageNum == 2)
        {
            v = col.ClosestPointOnBounds(Hand_R.transform.position);
        }
        else if (StageNum == 3)
        {
            v = col.ClosestPointOnBounds(Leg_R.transform.position);
        }
        var bloodEffect = ResourceManager.GetInstance().GetObjInstance<GameObject>("Common/BloodEffect");
        bloodEffect.transform.position = v;
        character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });
        //顿帧
        character.anim.speed = 0f;
        TimeManager.GetInstance().AddOnceTimer(this, 0.05f, () =>
        {
            character.anim.speed = 1;
        });
        var dir = (character.trans.forward).normalized;
        dir.y = 0;
        target.physic.Move(dir.normalized * 0.1f, 0.1f);
    }

    protected override void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.DoAtk);
    }

    public override void OnBack()
    {
        base.OnBack();
        character.anim.speed = 1;
        if (StageNum == 1)
        {
            Hand_L.OnTriggerEnterCall -= DoDamage;
            Hand_L.enabled = false;
        }
        else if (StageNum == 2)
        {
            Hand_R.OnTriggerEnterCall -= DoDamage;
            Hand_R.enabled = false;

        }
        else if (StageNum == 3)
        {
            Leg_R.OnTriggerEnterCall -= DoDamage;
            Leg_R.enabled = false;
        }

    }

    public override void OnExit()
    {
        base.OnExit();
        character.anim.speed = 1;
    }

    bool triggered = false;
    public void Atk()
    {
        triggered = false;
        if (StageNum == 1)
        {
            Hand_L.enabled = true;
            Hand_L.OnTriggerEnterCall += DoDamage;
        }
        else if (StageNum == 2)
        {
            Hand_R.enabled = true;
            Hand_R.OnTriggerEnterCall += DoDamage;

        }
        else if (StageNum == 3)
        {
            Leg_R.enabled = true;
            Leg_R.OnTriggerEnterCall += DoDamage;
        }
    }
}