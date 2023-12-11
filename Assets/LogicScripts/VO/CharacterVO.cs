﻿using System;
using System.Collections.Generic;

public class CharacterVO
{
    public cfg.Character character;

    public int id { get { return character.Id; } set { id = value; } }

    public string characterName { get { return character.Name; } set { characterName = value; } }

    public string modelName { get { return character.ModePath; } set { modelName = value; } }
    public string skills { get { return skills; } set { skills = value; } }
    public string stateAnims { get { return stateAnims; } set { stateAnims = value; } }

    public CharacterVO()
    {

    }

    public string GetStateAnimKey(string stateName)
    {
        var anims = StateConfiger.GetInstance().GetStateByType(stateName).stateAnimName;
        if (character.StateAnims == string.Empty)
        {
            //无覆盖状态动画
            return anims;
        }
        else
        {
            //有状态动画覆盖
            var list = character.StateAnims.Split(';');
            for (int i = 0; i < list.Length; i++)
            {
                var stateInfo = list[i].Split(':');
                //状态名称
                var stateNameStr = stateInfo[0];
                //动画key
                var animKey = stateInfo[1];
                if (stateNameStr == stateName)
                {
                    return animKey;
                }
            }
        }
        return anims;
    }


    public List<int> GetSkillArr()
    {
        return character.Skills;
    }


    /// <summary>
    /// 根据技能类型技能
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<int> GetSkillByType(cfg.SkillTags tag)
    {
        List<int> result = new List<int>();
        foreach (var item in GetSkillArr())
        {
            var skill = SkillConfiger.GetInstance().GetSkillById(item);
            if (skill.skillTags.IndexOf(tag) != -1)
            {
                result.Add(item);
            }
        }
        return result;
    }

    /*    /// <summary>
        /// 获取所有的状态动画 混合类型
        /// </summary>
        /// <returns></returns>
        public List<AnimInfoData> GetBlendAnimInfos()
        {
            return GetAnimInfosByType(AnimType.BLEND);
        }

        /// <summary>
        /// 获取所有的状态动画 片段类型
        /// </summary>
        /// <returns></returns>
        public List<AnimInfoData> GetClipAnimInfos()
        {
            return GetAnimInfosByType(AnimType.CLIP);
        }

        private List<AnimInfoData> GetAnimInfosByType(int type)
        {
            var states = StateConfiger.GetInstance().GetAllStateData();
            List<AnimInfoData> result = new List<AnimInfoData>();
            foreach (var item in states)
            {
                if (item.stateAnimName != string.Empty)
                {
                    AnimInfoData data = null;// RegAnimKey.GetKeys(item.stateAnimName);
                    if (data.animType == type)
                    {
                        result.Add(data);
                    }
                }

            }
            return result;
        }*/
}