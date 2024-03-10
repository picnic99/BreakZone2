using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class PlayerManager : Manager<PlayerManager>
    {

        public int curIdIndex = 1;
        Dictionary<int, Player> datas = new Dictionary<int, Player>();

        public Player FindPlayerFromDB(string username, string password)
        {
            //UID PWD 作用从数据库查找到玩家数据

            Player p = new Player();
            //从数据库读取这个玩家的数据 存入实体中
            p.playerId = curIdIndex++;
            datas.Add(p.playerId, p);
            return p;
        }

        public Player FindPlayer(int playerId)
        {
            if (datas.ContainsKey(playerId))
            {
                return datas[playerId];
            }
            return null;
        }

        public List<Player> GetAllPlayer()
        {
            List<Player> result = new List<Player>();
            foreach (var item in datas)
            {
                result.Add(item.Value);
            }
            return result;
        }


        /// <summary>
        /// 获取该玩家能看到的玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public List<Player> GetAOIPlayers(int playerId)
        {
            return new List<Player>();
        }

        public List<Player> GetAllPlayerInScene(int sceneId)
        {
            List<Player> result = new List<Player>();
            foreach (var item in datas)
            {
                if (item.Value.lastStaySceneId == sceneId)
                {
                    result.Add(item.Value);
                }
            }
            return result;
        }

        public List<Player> GetAllPlayerInSceneNoSelf(int sceneId,int selfId)
        {
            List<Player> result = new List<Player>();
            foreach (var item in datas)
            {
                if (item.Value.lastStaySceneId == sceneId && item.Value.playerId != selfId)
                {
                    result.Add(item.Value);
                }
            }
            return result;
        }


        public void RemovePlayer(int playerId)
        {
            //需要有各登出清理
            datas.Remove(playerId);
        }
    }
}
