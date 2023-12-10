using DG.Tweening;
using System;
using UnityEngine;

public class JinZhongZhaoSkill : Skill
{

    public JinZhongZhaoSkill()
    {
        skillDurationTime = stateDurationTime = 0.25f;
    }

    public override void OnEnter()
    {
        PlayAnim("skill2");
        base.OnEnter();
    }
    public override void OnTrigger()
    {
        base.OnTrigger();
        this.character.physic.Move(GameContext.GetDirByInput(character).normalized * 2f, skillDurationTime);
    }
}