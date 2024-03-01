using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class LoginInRep : RepProtocol
    {
        public int playerId;
        public LoginInRep(int result, int playerId) : base(result)
        {
            protocolId = (int)ProtocolId.CLENT_LOGIN_IN_REP;
            this.playerId = playerId;

        }
    }
}
