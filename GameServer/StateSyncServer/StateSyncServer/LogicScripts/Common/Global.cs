using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Common
{
    class Global
    {
        //每秒帧数
        public static int FrameRate = 20;
        public static float FixedFrameTimeS = 0.05f; // 1000/20
        public static int FixedFrameTimeMS = 50; // 1000/20

        private static Dictionary<int, TcpClient> PlayerSocketMap = new Dictionary<int, TcpClient>();
    
        public static int GetPlayerIdByClient(TcpClient client)
        {
            int id = 0;
            foreach (var item in PlayerSocketMap)
            {
                if(item.Value == client)
                {
                    id = item.Key;
                    break;
                }
            }
            return id;
        }

        public static Player GetPlayerByClient(TcpClient client)
        {
           int playerId = GetPlayerIdByClient(client);
            if(playerId > 0)
            {
                var p = PlayerManager.GetInstance().FindPlayer(playerId);
                return p;
            }
            return null;
        }

        public static TcpClient GetClientByPlayerId(int playerId)
        {
            if (PlayerSocketMap.ContainsKey(playerId))
            {
                return PlayerSocketMap[playerId];
            }
            return null;
        }

        public static void AddPlayerClientMap(int playerId,TcpClient client)
        {
            PlayerSocketMap.Add(playerId, client);
        }

        public static bool RemovePlayerClientMap(int playerId)
        {
            if (PlayerSocketMap.ContainsKey(playerId))
            {
                return PlayerSocketMap.Remove(playerId);
            }
            return false;
        }

        public static bool RemovePlayerClientMap(TcpClient client)
        {
            int key = 0;
            foreach (var item in PlayerSocketMap)
            {
                if (item.Value == client)
                {
                    key = item.Key;
                    break;
                }
            }
            if (key > 0)
            {
                return RemovePlayerClientMap(key);
            }
            return false;
        }
    }
}
