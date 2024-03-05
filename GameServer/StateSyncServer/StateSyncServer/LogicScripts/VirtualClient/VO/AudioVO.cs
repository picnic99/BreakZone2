using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.VO
{
    public class AudioVO
    {
        public cfg.Audio audio;

        public string GetKeyStr()
        {
            int crtId = audio.CharacterId.ToString() == "" ? 0 : audio.CharacterId;
            int SkillId = audio.SkillId.ToString() == "" ? 0 : audio.SkillId;
            int StateId = audio.StateId.ToString() == "" ? 0 : audio.StateId;

            return $"{crtId}_{SkillId}_{StateId}_{audio.Keyword}";
        }

        public bool IsAllPlay()
        {
            return audio.PlayMode == 1;
        }
        public bool IsRandomPlay()
        {
            return audio.PlayMode == 2;
        }
    }
}
