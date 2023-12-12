using System;
using UnityEngine;

public class BaseAttack : Skill
{
    //再次触发最大等待时间
    public float maxWaitTime = 0.5f;

    public ColliderHelper Hand_R;

    public BaseAttack()
    {
        //允许再次触发
        CanTriggerAgain = true;
    }

    public override void OnEnter()
    {
        Hand_R = character.anim.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<ColliderHelper>();
        Hand_R.enabled = false;

        if (StageNum >= 3)
        {
            StageNum = 0;
        }

        //播放动画前 考虑动画覆盖问题
        //动画覆盖分为两种 1 状态动画 2 技能动画
        AnimCoverVO vo = character.animCoverData.GetHead(belongState);
        if (vo != null)
        {
            PlayAnim(vo.animName);
        }
        else
        {
            PlayAnim(skillData.GetAnimKey(StageNum));
        }
        base.OnEnter();

        //stateDurationTime = curAnimLength;
        skillDurationTime = stateDurationTime + maxWaitTime;
    }

    bool triggered = false;
    public void Atk()
    {
        triggered = false;
        Hand_R.enabled = true;
        Hand_R.OnTriggerEnterCall += DoDamage;
        character.eventDispatcher.Event(CharacterEvent.PRE_ATK);
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
        v = col.ClosestPointOnBounds(Hand_R.transform.position);

        var bloodEffect = ResourceManager.GetInstance().GetObjInstance<GameObject>("Common/BloodEffect");
        bloodEffect.transform.position = v;
        character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });
        //顿帧
        character.anim.speed = 0f;
        TimeManager.GetInstance().AddOnceTimer(this, 0.05f, () =>
        {
            character.anim.speed = 1;
        });
        TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
        {
            GameObject.Destroy(bloodEffect);
        });

        //受击位移
/*        var dir = (character.trans.forward).normalized;
        dir.y = 0;
        target.physic.Move(dir.normalized * 0.1f, 0.1f);*/
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        Atk();
    }

    protected override void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.DoAtk);
    }

    public override void OnBack()
    {
        base.OnBack();
        character.anim.speed = 1;
        Hand_R.OnTriggerEnterCall -= DoDamage;
        Hand_R.enabled = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        character.anim.speed = 1;
    }
}