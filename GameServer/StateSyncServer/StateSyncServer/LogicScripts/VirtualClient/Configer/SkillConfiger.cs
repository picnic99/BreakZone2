using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    public class SkillConfiger : Singleton<SkillConfiger>
    {
        private List<SkillVO> List;

        public void Init()
        {
            if (List == null) List = new List<SkillVO>();
            foreach (var item in Configer.Tables.TbSkill.DataList)
            {
                var anim = new SkillVO();
                anim.skill = item;
                List.Add(anim);
            }
        }
        public SkillVO GetSkillById(int id)
        {
            var vo = List.Find((item) => { return item.Id == id; });
            return vo;
        }
    }
}
