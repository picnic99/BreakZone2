using Newtonsoft.Json;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using StateSyncServer.LogicScripts.VirtualClient.VO.Anim;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    public class AnimConfiger : Bases.Singleton<AnimConfiger>
    {
        private List<AnimationVO> List;

        private List<AnimClipDataInfo> clipDataList;
        private Dictionary<string, AnimClipDataInfo> clipDataDic;

        public void Init()
        {
            if (List == null) List = new List<AnimationVO>();

            foreach (var item in Configer.Tables.TbAnimation.DataList)
            {
                var anim = new AnimationVO();
                anim.animation = item;
                List.Add(anim);
            }

            string animJson = ResourceManager.GetInstance().GetAnimClipDataRes();
            clipDataList = JsonConvert.DeserializeObject<List<AnimClipDataInfo>>(animJson);
            clipDataDic = new Dictionary<string, AnimClipDataInfo>();
            foreach (var item in clipDataList)
            {
                clipDataDic.Add(item.animName, item);
            }
        }
        public AnimationVO GetAnimByAnimKey(string key)
        {
            var vo = List.Find((item) => { return item.animation.Key == key; });
            return vo;
        }

        public AnimClipDataInfo GetClipDataByName(string animName)
        {
            if (clipDataDic == null) return null;
            if (clipDataDic.ContainsKey(animName)) return clipDataDic[animName];
            return null;
        }
    }
}