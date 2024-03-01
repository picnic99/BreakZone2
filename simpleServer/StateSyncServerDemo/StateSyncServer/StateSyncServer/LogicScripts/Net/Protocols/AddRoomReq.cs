using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class AddRoomReq : ReqProtocol
    {
        public int playerId;
        public int roomId;
        public AddRoomReq(int playerId,int roomId)
        {
            this.protocolId = (int)ProtocolId.CLENT_ADD_ROOM_REQ;
            this.playerId = playerId;
            this.roomId = roomId;
        }
    }
}
