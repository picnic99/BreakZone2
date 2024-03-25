using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 技能管理器
/// </summary>
public class SkillManager : Manager<SkillManager>
{
    public class skillCD
    {
        float time;

        public skillCD(float time)
        {
            this.time = time;
        }
        public void Reduce()
        {
            time -= Time.deltaTime;
        }

        public float GetTime()
        {
            return time;
        }
        public bool InCD()
        {
            return time > 0;
        }
    }

    private Queue<SkillInstance2> skillInsPool = new Queue<SkillInstance2>();

    private Dictionary<int, skillCD> skillCDRecordDic = new Dictionary<int, skillCD>();

    public Skill CreateSkill(int skillId, _Character character = null,string stateName = null)
    {
        Type type = RegSkillClass.GetInstance().GetType(skillId);
        Skill skill = (Skill)type.Assembly.CreateInstance(type.Name);
        var vo = SkillConfiger.GetInstance().GetSkillById(skillId);
        skill.skillData = vo;
        if (stateName != null) skill.belongState = stateName;
        //if (character != null) skill.Character = character;
        AddSkillCoolDown(skill);
        return skill;
    }

    public void AddSkillCoolDown(Skill skill)
    {
        if (!skillCDRecordDic.ContainsKey(skill.skillData.Id))
        {
            skillCDRecordDic.Add(skill.skillData.Id, new skillCD(skill.skillData.baseCoolDown));
        }
        else if (!InCoolDown(skill.skillData.Id))
        {
            skillCDRecordDic[skill.skillData.Id] = new skillCD(skill.skillData.baseCoolDown);

        }
    }

    public float GetSkillCoolDown(int skillId)
    {
        if (skillCDRecordDic.ContainsKey(skillId))
        {
            return skillCDRecordDic[skillId].GetTime();
        }
        return 0;
    }

    public void UpdateCD()
    {
        foreach (var item in skillCDRecordDic)
        {
            item.Value.Reduce();
        }
    }

    /// <summary>
    /// 技能在冷却中？
    /// </summary>
    /// <param name="skillId"></param>
    public bool InCoolDown(int skillId)
    {
        if (!skillCDRecordDic.ContainsKey(skillId)) return false;
        return skillCDRecordDic[skillId].InCD();
    }

    /// <summary>
    /// 创建一个技能实例
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public SkillInstance2 CreateSkillInstance(Skill skill, string name, Vector3 createPos, Vector3 moveTo, float moveSpeed)
    {
        SkillInstance2 ins = null;
        if (skillInsPool.Count <= 0)
        {
            ins = new SkillInstance2();
        }
        else
        {
            ins = skillInsPool.Dequeue();
        }
        ins.Init(skill, name, createPos, moveTo, moveSpeed);
        return ins;
    }

    public override void Init()
    {
        MonoBridge.GetInstance().AddCall(UpdateCD);
        base.Init();
    }

    public void RecoverSkillInstance(SkillInstance2 ins)
    {
        skillInsPool.Enqueue(ins);
    }
}