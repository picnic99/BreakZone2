using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class PlayerManager:Manager<PlayerManager>
    {
        Dictionary<string, Player> datas = new Dictionary<string, Player>();

        public Player GetPlayer(string username,string password)
        {
            //从数据库读取这个玩家的数据 存入实体中
            Player p = new Player(username, password);
            datas.Add(p.username, p);
            return p;
        }

        public void RemovePlayer(string username)
        {
            //需要有各登出清理
            datas.Remove(username);
        }
    }
}
