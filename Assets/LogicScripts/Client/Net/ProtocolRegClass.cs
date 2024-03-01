using Assets.LogicScripts.Client.Net.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.Net
{
    class ProtocolRegClass
    {
        public static ProtocolRegClass Instance;

        public Dictionary<ProtocolId, Type> dic = new Dictionary<ProtocolId, Type>();
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
            /*            dic.Add(ProtocolId.CLENT_LOGIN_IN_REQ, typeof(LoginInReq));
                        dic.Add(ProtocolId.CLENT_LOGIN_IN_REP, typeof(LoginInRep));
                        dic.Add(ProtocolId.CLENT_PLAYER_OPT_REQ, typeof(PlayerOptReq));
                        dic.Add(ProtocolId.CLENT_PLAYER_OPT_REP, typeof(PlayerOptRep));
                        dic.Add(ProtocolId.CLENT_ADD_ROOM_REQ, typeof(AddRoomReq));
                        dic.Add(ProtocolId.CLENT_ADD_ROOM_REP, typeof(AddRoomRep));
                        dic.Add(ProtocolId.CLENT_GAME_DATA_NTF, typeof(GameDataNtf));
                        dic.Add(ProtocolId.CLENT_PLAYER_ADD_NTF, typeof(PlayerAddNtf));
                        dic.Add(ProtocolId.CLENT_BULLET_ADD_NTF, typeof(AddBulletNtf));*/

            dic.Add(ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ, typeof(GamePlayerOptReq));
            dic.Add(ProtocolId.CLIENT_GAME_PLAYER_OPT_REP, typeof(GamePlayerOptRep));

        }

        public Type GetType(ProtocolId key)
        {
            if (!dic.ContainsKey(key))
            {
                Debug.LogError("regClass not found" + key + ", pls check register");
                return null;
            }
            Type t = dic[key];
            return t;
        }
    }
}
