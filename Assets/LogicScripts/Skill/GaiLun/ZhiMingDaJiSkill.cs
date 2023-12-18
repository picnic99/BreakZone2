//致命打击 提升移动速度 移除所有减速效果 下次攻击附带额外伤害以及沉默效果
using System;
using System.Collections.Generic;
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
        //character.animCoverData.Add(StateType.Run, moveCover);
        character.animCoverData.Add(StateType.DoAtk, atkCover);
        AddBuff(new Character[] { character }, new BuffVO("致命打击加移速", skillDurationTime), (buff) =>
        {
            buff.AddBuffComponent(buff.AddPropertyBuff(skillDurationTime, new PropertyBuffVO(PropertyType.MOVESPEED, false, 0.35f)));
        }, (args) =>
        {
            character.animCoverData.Remove(StateType.Move, moveCover);
            character.animCoverData.Remove(StateType.DoAtk, atkCover);
            //character.animCoverData.Remove(StateType.Run, atkCover);
        });

        //攻击施加沉默状态
        character.eventDispatcher.On(CharacterEvent.PRE_ATK, ATK_BUFF);
    }

    protected override void OnEnd()
    {
        character.eventDispatcher.Off(CharacterEvent.PRE_ATK, ATK_BUFF);
        base.OnEnd();
    }

    //添加沉默状态
    private void ATK_BUFF(object[] target)
    {
        character.animCoverData.Remove(StateType.DoAtk, atkCover);
        character.animCoverData.Remove(StateType.Move, moveCover);

        new ZhiMingDaJiInstance(this, DoTrigger, 0);
        new ZhiMingDaJiInstance(this, DoTrigger, 15);
        new ZhiMingDaJiInstance(this, DoTrigger, -15);
        character.eventDispatcher.Off(CharacterEvent.PRE_ATK, ATK_BUFF);
        skillDurationTime = 0f;
    }

    public void DoTrigger(Character target)
    {
        DoDamage(new Character[] { target }, 25);
    }

    public override string GetDesc()
    {
        return "当前技能为：致命打击，剩余时间为：" + skillDurationTime.ToString("F2");
    }
}

public class ZhiMingDaJiInstance : SkillInstance
{
    //飞轮有两道伤害
    List<Character> triggeredCharacterList = new List<Character>();

    float moveOffset = 0;
    float maxTime = 2;

    public ZhiMingDaJiInstance(Skill skill,Action<Character> call, float moveOffset)
    {
        this.RootSkill = skill;
        this.instancePath = "Skill/ZhiMingDaJi";
        this.durationTime = maxTime;
        this.maxTriggerTarget = 99;
        this.enterCall = call;
        this.instanceObj = ResourceManager.GetInstance().GetObjInstance<GameObject>(instancePath);
        this.instanceObj.SetActive(false);
        this.moveOffset = moveOffset;
        this.Init();
    }

    public override void AddBehaviour()
    {
        TimeManager.GetInstance().AddLoopTimer(this, 0.25f, () =>
        {
            if (durationTime <= 0)
            {
                End();
                return;
            }
            DoMove();
            durationTime -= Time.deltaTime;
        });
    }

    private void DoMove()
    {
        this.instanceObj.SetActive(true);

        // TODO 返回时追踪角色位置
        if (this.durationTime <= maxTime / 2)
        {
            this.instanceObj.transform.position -= this.instanceObj.transform.forward * Time.deltaTime * 30f;
            this.instanceObj.transform.localScale -= Vector3.one * Time.deltaTime * 3f;
        }
        else
        {
            this.instanceObj.transform.position += this.instanceObj.transform.forward * Time.deltaTime * 30f;
            this.instanceObj.transform.localScale += Vector3.one * Time.deltaTime * 3f;
        }
    }



    public override void InitTransform()
    {
        instanceObj.transform.position = RootSkill.character.trans.position + RootSkill.character.trans.forward * 1f;
        instanceObj.transform.forward = RootSkill.character.trans.forward;
        instanceObj.transform.RotateAround(instanceObj.transform.position,Vector3.up,moveOffset);
    }

    public override void InvokeEnterTrigger(Character target)
    {
        enterCall.Invoke(target);
    }
}