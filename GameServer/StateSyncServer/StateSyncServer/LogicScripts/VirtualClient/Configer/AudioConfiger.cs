using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    public class AudioConfiger : Singleton<AudioConfiger>
    {
        private List<AudioVO> List;

        public void Init()
        {
            if (List == null) List = new List<AudioVO>();

            foreach (var item in Configer.Tables.TbAudio.DataList)
            {
                var vo = new AudioVO();
                vo.audio = item;
                List.Add(vo);
            }
        }
        public AudioVO GetAudioData(int crtId, int skillId, int stateId, string keyword = "")
        {
            var vo = List.Find((item) => { return item.GetKeyStr() == GetKeyStr(crtId, skillId, stateId, keyword); });
            return vo;
        }
        public string GetKeyStr(int crtId = 0, int skillId = 0, int stateId = 0, string keyword = "")
        {
            return $"{crtId}_{skillId}_{stateId}_{keyword}";
        }
    }
}
