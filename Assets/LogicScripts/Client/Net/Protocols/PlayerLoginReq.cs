
using Assets.LogicScripts.Client.Enum;
using UnityEngine;

namespace Assets.LogicScripts.Client.Net.Protocols
{
    class PlayerLoginReq : Protocol
    {
        public string username;
        public string password;
        public int type; //1 登录 0 注销
        public PlayerLoginReq()
        {
            protocolId = ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ;
        }

        public PlayerLoginReq(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
