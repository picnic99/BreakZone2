using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class GetGameDataReq:ReqProtocol
    {
        public int playerId;

        public GetGameDataReq(int playerId)
        {
            this.protocolId = (int)ProtocolId.CLENT_GET_GAME_DATA_REQ;
            this.playerId = playerId;
        }
    }
}
