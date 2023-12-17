using UnityEngine;

public class ZhengYiSkill : Skill
{
    //private Character target;
    public override void OnEnter()
    {
        //播放正义动画
        PlayAnim(skillData.GetAnimKey(0));
        base.OnEnter();
        stateDurationTime = skillDurationTime = 1.5f;
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        //character.scan.ShowRange(360, 1.5f);
        Character[] enemys = character.scan.CheckShphere(character.trans.position, character.trans.forward, "0,360", 1.5f, RangeType.ENEMY);
        DoDamage(enemys, 100);
        EffectManager.GetInstance().PlayEffect("Skill/ZhengYi", 3f, null, character.trans.position + character.trans.forward * 7f, character.trans.forward, new Vector3(1f, 1f, 1f));
        foreach (var target in enemys)
        {
            var dir = (target.trans.position - character.trans.position).normalized;
            dir.y = 2f;
            target.physic.Move(dir.normalized * 0.2f, 0.1f);
        }
    }

/*    protected override void OnEnd()
    {
        //移除掉来自技能施加的所有buff效果
        BuffManager.GetInstance().RemoveAllBuffFromSkill(character, this);
        SetReleaseOver();
        base.OnEnd();
    }*/

    public override void OnExit()
    {
        BuffManager.GetInstance().RemoveAllBuffFromSkill(character, this);
        base.OnExit();
    }

    public override string GetDesc()
    {
        return "当前技能为：正义，剩余时间为：" + skillDurationTime.ToString("F2");
    }
}