using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System.Collections.Generic;

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