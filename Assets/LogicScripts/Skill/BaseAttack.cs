using System;
using UnityEngine;

public class BaseAttack : Skill
{
    public float maxWaitTime = 0.5f;

    public BaseAttack()
    {
        stateDurationTime = 0.8f;
        skillDurationTime = stateDurationTime + maxWaitTime;
        CanTriggerAgain = true;
    }

    public override void OnEnter()
    {
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
        skillDurationTime = stateDurationTime + maxWaitTime;
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        character.scan.ShowRange(90, 2f);
        Character[] targets = character.scan.CheckShphere(character.trans.position, character.trans.forward, "-45,45", 2f, RangeType.ENEMY);
        DoDamage(targets, character.property.Atk);
        //EffectManager.GetInstance().PlayEffect("Skill/NearBaseAtk", 1f, null, character.trans.position, character.trans.forward, new Vector3(0.2f, 0.2f, 0.2f));
        character.eventDispatcher.Event(CharacterEvent.ATK, targets);
        if (targets.Length > 0)
        {
            //顿帧
            character.anim.speed = 0f;
            TimeManager.GetInstance().AddOnceTimer(this, 0.05f, () =>
            {
                character.anim.speed = 1;
            });
        }
        foreach (var target in targets)
        {
            var dir = (character.trans.forward).normalized;
            dir.y = 0;
            target.physic.Move(dir.normalized * 0.1f, 0.1f);
        }
    }

    protected override void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.DoAtk);
    }

    public override void OnBack()
    {
        base.OnBack();
        character.anim.speed = 1;
    }

    public override void OnExit()
    {
        base.OnExit();
        character.anim.speed = 1;
    }
}