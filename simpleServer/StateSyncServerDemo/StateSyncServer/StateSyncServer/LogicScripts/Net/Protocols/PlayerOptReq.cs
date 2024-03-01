using StateSyncServer.LogicScripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class PlayerOptReq : ReqProtocol
    {
        public int playerId;
        public FightActionEnum type;
        public float rot;
        public int skillId;

        public PlayerOptReq(int playerId, FightActionEnum type, float rot = 0, int skill = 1)
        {
            this.protocolId = (int)ProtocolId.CLENT_PLAYER_OPT_REQ;
            this.playerId = playerId;
            this.type = type;
            this.skillId = skill;
            this.rot = rot;
        }
    }
}
