using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class PlayerAddNtf : Protocol
    {
        public int roomId;
        public int[] playerIds;

        public PlayerAddNtf(int roomId,int[] playerIds)
        {
            protocolId = (int)ProtocolId.CLENT_PLAYER_ADD_NTF;
            this.roomId = roomId;
            this.playerIds = playerIds;
        }
    }
}
