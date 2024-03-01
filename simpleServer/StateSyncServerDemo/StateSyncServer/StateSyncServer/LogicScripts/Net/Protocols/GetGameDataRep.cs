using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class GetGameDataRep : RepProtocol
    {
        public GetGameDataRep(int result) : base(result)
        {
            protocolId = (int)ProtocolId.CLENT_GET_GAME_DATA_REP;
        }
    }
}
