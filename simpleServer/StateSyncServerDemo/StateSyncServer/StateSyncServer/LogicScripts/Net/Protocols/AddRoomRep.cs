using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class AddRoomRep:RepProtocol
    {
        public int roomId;
        public int[] playerIds;

        public AddRoomRep(int result,int roomId,int[] playerIds) :base(result)
        {
            this.protocolId = (int)ProtocolId.CLENT_ADD_ROOM_REP;
            this.roomId = roomId;
            this.playerIds = playerIds;
        }
    }
}
