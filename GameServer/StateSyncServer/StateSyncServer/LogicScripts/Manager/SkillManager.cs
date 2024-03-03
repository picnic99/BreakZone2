using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class SkillManager : Manager<SkillManager>
    {
        public SkillManager()
        {

        }
        private List<Skill> skillList = new List<Skill>();

        public void AddSkill(Skill skill)
        {
            this.skillList.Add(skill);
        }

        public void RemoveSkill(Skill skill)
        {
            skillList.Remove(skill);
        }

        public void Tick()
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].Tick();
            }
        }

    }
}
