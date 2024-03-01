using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class LoginInReq : ReqProtocol
    {
        public int playerId;

        public LoginInReq(int playerId)
        {
            this.protocolId = (int)ProtocolId.CLENT_LOGIN_IN_REQ;
            this.playerId = playerId;
        }
    }
}
