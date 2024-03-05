using Google.Protobuf;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.PB
{
    class RegProtocol
    {
        private static Dictionary<int, Type> typeDic;

        public static void Init()
        {
            typeDic = new Dictionary<int, Type>();
            RegProtocolData(ProtocolId.CLIENT_PLAYER_LOGIN_REQ, typeof(PlayerLoginReq));
            RegProtocolData(ProtocolId.CLIENT_PLAYER_LOGIN_REP, typeof(PlayerLoginRep));
            RegProtocolData(ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF, typeof(PlayerBaseInfoNtf));
            RegProtocolData(ProtocolId.CLIENT_SELECT_ROLE_REQ, typeof(SelectCrtReq));
            RegProtocolData(ProtocolId.CLIENT_SELECT_ROLE_REP, typeof(SelectCrtRep));
            RegProtocolData(ProtocolId.CLIENT_ENTER_SCENE_REQ, typeof(EnterSceneReq));
            RegProtocolData(ProtocolId.CLIENT_ENTER_SCENE_REP, typeof(EnterSceneRep));
            RegProtocolData(ProtocolId.CLENT_GET_GAME_DATA_REQ, typeof(GetGameDataReq));
            RegProtocolData(ProtocolId.CLENT_GET_GAME_DATA_REP, typeof(GetGameDataRep));
            RegProtocolData(ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ, typeof(GamePlayerOptReq));
            RegProtocolData(ProtocolId.CLIENT_GAME_PLAYER_OPT_REP, typeof(GamePlayerOptRep));
            RegProtocolData(ProtocolId.CLIENT_ANIM_PLAY_NTF, typeof(GameAnimPlayNtf));
            RegProtocolData(ProtocolId.CLIENT_GAME_PLAYER_INPUT_REQ, typeof(GamePlayerInputReq));
            RegProtocolData(ProtocolId.CLIENT_GAME_PLAYER_INPUT_NTF, typeof(GamePlayerInputNtf));
            RegProtocolData(ProtocolId.CLENT_SYNC_GAME_DATA_NTF, typeof(SyncGameDataNtf));
        }

        public static Type GetProtocolType(int id)
        {
            if (typeDic.ContainsKey(id))
            {
                return typeDic[id];
            }

            return null;
        }
        public static int GetProtocolId(IMessage type)
        {
            int id = 0;
            foreach (var item in typeDic)
            {
                if(item.Value.IsInstanceOfType(type))
                {
                    id = item.Key;
                    break;
                }
            }
            return id;
        }


        private static void RegProtocolData(int protoId, Type type)
        {
            typeDic.Add(protoId, type);
        }
    }
}
