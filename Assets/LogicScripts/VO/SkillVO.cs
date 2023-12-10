using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillVO
{
    public cfg.Skill skill;

    public int Id { get { return skill.Id; }}
    public string SkillName { get { return skill.Name; } }

    public float baseCoolDown { get { return skill.Cd; } }
    public bool IsInstantSkill { get { return skill.IsInstantSkill; } }
    /// <summary>
    /// 技能的扩展参数
    /// </summary>
    public string SkillParam { get { return skill.ExtraParams; } }
    /// <summary>
    /// 覆盖互斥状态 特殊情况不使用状态表的那一套
    /// </summary>
    public List<int> mutexState { get { return skill.MutexState; } }
    /// <summary>
    /// 可以打断该技能后摇的技能id列表
    /// </summary>
    public List<int> backBreakSkills { get { return skill.BackBreakSkills; } }

    public List<cfg.SkillTags> skillTags { get { return skill.Tags; } }
    public List<String> AnimKeys { get { return skill.AnimKeys; } }


    public SkillVO()
    {

    }

    /// <summary>
    /// 根据技能当前段数获取动画key
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetAnimKeyBySkillIndex(int index)
    {
        if (skill.AnimKeys.Count <= index)
        {
            Debug.LogError($"Skill配置 animKeys为{skill.AnimKeys.Count},当前需要的下标为{index}");
            return "";
        }
        return skill.AnimKeys[index];
    }

    public bool IsOnlySkill()
    {
        return skill.IsOnly;
    }

    public string GetAnimKey(int index)
    {
        return GetAnimKeyBySkillIndex(index);
    }

    public List<int> GetMutexList()
    {
        return skill.MutexState;
    }

    /// <summary>
    /// 获取技能前摇时间
    /// </summary>
    /// <param name="stageNum">阶段</param>
    /// <returns></returns>
    public float GetFrontTime(int stageNum)
    {
        var animVO = GetAnimVOByStage(stageNum);
        return animVO.animation.FrontTime;
    }

    /// <summary>
    /// 获取技能后摇时间
    /// </summary>
    /// <param name="stageNum">阶段</param>
    /// <returns></returns>
    public float GetBackTime(int stageNum)
    {
        var animVO = GetAnimVOByStage(stageNum);
        return animVO.animation.BackTime;
    }

    /// <summary>
    /// 根据阶段获取动画VO
    /// </summary>
    /// <param name="stageNum"></param>
    /// <returns></returns>
    public AnimationVO GetAnimVOByStage(int stageNum)
    {
        string animKey = GetAnimKeyBySkillIndex(stageNum);
        if (animKey != string.Empty)
        {
            var animVO = AnimConfiger.GetInstance().GetAnimByAnimKey(animKey);
            return animVO;
        }
        return null;
    }

    /// <summary>
    /// 能够打断后摇的技能id
    /// </summary>
    /// <returns></returns>
    public List<int> GetBackBreakList()
    {
        return skill.BackBreakSkills;
    }

}