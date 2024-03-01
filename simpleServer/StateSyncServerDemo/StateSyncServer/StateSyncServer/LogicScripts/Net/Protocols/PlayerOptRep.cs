using StateSyncServer.LogicScripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class PlayerOptRep : RepProtocol
    {
        public int playerId;
        public FightActionEnum type;
        public float rot;

        public PlayerOptRep(int result, int playerId, FightActionEnum type, float rot) : base(result)
        {
            protocolId = (int)ProtocolId.CLENT_PLAYER_OPT_REP;
            this.playerId = playerId;
            this.type = type;
            this.rot = rot;
        }
    }
}
