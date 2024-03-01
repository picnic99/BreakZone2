using StateSyncServer.LogicScripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols.Base
{
    class ProtocolRegClass
    {
        public static ProtocolRegClass Instance;

        public Dictionary<int, Type> dic = new Dictionary<int, Type>();
        public static ProtocolRegClass GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ProtocolRegClass();
                Instance.Init();
            }
            return Instance;
        }

        private void Init()
        {
            dic.Add((int)ProtocolId.CLENT_LOGIN_IN_REQ, typeof(LoginInReq));
            dic.Add((int)ProtocolId.CLENT_LOGIN_IN_REP, typeof(LoginInRep));
            dic.Add((int)ProtocolId.CLENT_PLAYER_OPT_REQ, typeof(PlayerOptReq));
            dic.Add((int)ProtocolId.CLENT_PLAYER_OPT_REP, typeof(PlayerOptRep));
            dic.Add((int)ProtocolId.CLENT_ADD_ROOM_REQ, typeof(AddRoomReq));
            dic.Add((int)ProtocolId.CLENT_ADD_ROOM_REP, typeof(AddRoomRep));
            dic.Add((int)ProtocolId.CLENT_GAME_DATA_NTF, typeof(GameDataNtf));
            dic.Add((int)ProtocolId.CLENT_PLAYER_ADD_NTF, typeof(PlayerAddNtf));
            dic.Add((int)ProtocolId.CLENT_BULLET_ADD_NTF, typeof(AddBulletNtf)); 
            dic.Add((int)ProtocolId.CLENT_GET_GAME_DATA_REQ, typeof(GetGameDataReq));
            dic.Add((int)ProtocolId.CLENT_GET_GAME_DATA_REP, typeof(GetGameDataRep));
        }

        public Type GetType(int key)
        {
            if (!dic.ContainsKey(key))
            {
                CommonUtils.Logout("regClass not found" + key + ", pls check register");
                return null;
            }
            Type t = dic[key];
            return t;
        }
    }
}
