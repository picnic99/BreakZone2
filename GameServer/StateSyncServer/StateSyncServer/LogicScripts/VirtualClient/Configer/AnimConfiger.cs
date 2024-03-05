using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    public class AnimConfiger : Singleton<AnimConfiger>
    {
        private List<AnimationVO> List;

        public void Init()
        {
            if (List == null) List = new List<AnimationVO>();

            foreach (var item in Configer.Tables.TbAnimation.DataList)
            {
                var anim = new AnimationVO();
                anim.animation = item;
                List.Add(anim);
            }
        }
        public AnimationVO GetAnimByAnimKey(string key)
        {
            var vo = List.Find((item) => { return item.animation.Key == key; });
            return vo;
        }
    }
}
