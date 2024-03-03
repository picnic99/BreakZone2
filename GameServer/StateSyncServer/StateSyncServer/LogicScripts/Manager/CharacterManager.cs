using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class CharacterManager:Manager<CharacterManager>
    {
        private Dictionary<int,Character> playerMap = new Dictionary<int,Character>();

        public CharacterManager()
        {

        }

        public Character CreatePlayer(int playerId)
        {
            var p = new Character(playerId);
            this.playerMap.Add(playerId,p);
            return p;
        }
        //移除角色实例
        public void DelPlayer(int playerId)
        {
            this.playerMap.Remove(playerId);
        }
        //查找角色
        public Character GetPlayer(int playerId)
        {
            if (this.playerMap.ContainsKey(playerId))
            {
            return this.playerMap[playerId];

            }
            return null;
        }

        //获取角色可视范围内的角色  包括自己
        public List<Character> GetAOIPlayer(int playerId)
        {
            List<Character> result = new List<Character>();
            var p = this.GetPlayer(playerId);
            if (p != null && p.data.roomId != 0)
            {
                var players = RoomManager.GetInstance().GetRoomPlayers(p.data.roomId);
                if (players != null && players.Count > 0)
                {
                    foreach (var other in players)
                    {
                        var len = Vector3.Distance(p.data.pos, other.data.pos);
                        if (len <= p.data.lookRange)
                        {
                            result.Add(other);
                        }

                    }
                }
            }
            return result;
        }

        public List<Character> GetAllPlayers()
        {
            return this.playerMap.Values.ToList();
        }

        public void Tick()
        {
            foreach (var item in playerMap)
            {
                item.Value.Tick();
            }
        }
    }
}
