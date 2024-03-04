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

        public Player GetPlayer(string username, string password)
        {
            //从数据库读取这个玩家的数据 存入实体中
            Player p = new Player(curIdIndex++, username, password);
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

        public void RemovePlayer(int playerId)
        {
            //需要有各登出清理
            datas.Remove(playerId);
        }
    }
}
