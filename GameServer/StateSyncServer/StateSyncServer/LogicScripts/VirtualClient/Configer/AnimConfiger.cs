using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System.Collections.Generic;

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