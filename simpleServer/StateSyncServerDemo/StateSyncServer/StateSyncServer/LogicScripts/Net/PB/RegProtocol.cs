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
