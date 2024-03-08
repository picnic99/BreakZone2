using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{

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
                time -= Global.FixedFrameTimeMS;
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

        private Dictionary<int, skillCD> skillCDRecordDic = new Dictionary<int, skillCD>();

        public Skill CreateSkill(int skillId, Character character = null, string stateName = null)
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


        public override void Init()
        {
            //MonoBridge.GetInstance().AddCall(UpdateCD);
            base.Init();
        }
    }
}