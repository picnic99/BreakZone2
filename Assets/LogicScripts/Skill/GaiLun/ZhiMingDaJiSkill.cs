//致命打击 提升移动速度 移除所有减速效果 下次攻击附带额外伤害以及沉默效果
using UnityEngine;

public class ZhiMingDaJiSkill : Skill
{
    private AnimCoverVO moveCover;
    private AnimCoverVO atkCover;

    public ZhiMingDaJiSkill()
    {
        skillDurationTime = 3f;
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        //移除所有的减速效果
        BuffManager.GetInstance().RemoveBuff(character);

        //动画覆盖 更改后续的移动动画和攻击动画
        moveCover = new AnimCoverVO("GAILUN_SWORD_RUN");
        atkCover = new AnimCoverVO("GAILUN_SWORD_HARD_ATK");

        character.animCoverData.Add(StateType.Move, moveCover);
        character.animCoverData.Add(StateType.Run, moveCover);
        character.animCoverData.Add(StateType.DoAtk, atkCover);
        AddBuff(new Character[] { character }, new BuffVO("致命打击加移速", skillDurationTime), (buff) =>
        {
            buff.AddBuffComponent(buff.AddPropertyBuff(skillDurationTime, new PropertyBuffVO(PropertyType.MOVESPEED, false, 0.35f)));
        }, (args) =>
        {
            character.animCoverData.Remove(StateType.Move, moveCover);
            character.animCoverData.Remove(StateType.DoAtk, atkCover);
            character.animCoverData.Remove(StateType.Run, atkCover);
        });

        //攻击施加沉默状态
        character.eventDispatcher.On(CharacterEvent.ATK, ATK_BUFF);
    }

    protected override void OnEnd()
    {
        character.eventDispatcher.Off(CharacterEvent.ATK, ATK_BUFF);
        base.OnEnd();
    }

    //添加沉默状态
    private void ATK_BUFF(object[] target)
    {
        character.animCoverData.Remove(StateType.DoAtk, atkCover);
        character.animCoverData.Remove(StateType.Move, moveCover);
        character.animCoverData.Remove(StateType.Run, moveCover);
        EffectManager.GetInstance().PlayEffect("Skill/ZhiMingDaJi", 1f, null, character.trans.position, character.trans.forward, new Vector3(0.1f, 0.1f, 0.1f));
        character.eventDispatcher.Off(CharacterEvent.ATK, ATK_BUFF);
        if (target == null || target.Length <= 0) return;
        Character cTarget = target[0] as Character;
        //伤害公式 base + 0.4 * atk + 0.2 * maxHp + 0.1 * curHp + 0.2 * defend 
        DoDamage(new Character[] { cTarget }, 25);
        AddState(new Character[] { cTarget }, character, StateType.Silence, 1.5f);
        skillDurationTime = 0f;
    }

    public override string GetDesc()
    {
        return "当前技能为：致命打击，剩余时间为：" + skillDurationTime.ToString("F2");
    }
}