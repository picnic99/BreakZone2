using Assets.LogicScripts.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Manager
{
    class PlayerManager : Manager<PlayerManager>
    {
        private Player self;
        public Player Self { get => self; set => self = value; }
        public bool SelfIsExist => self != null;

        Dictionary<int, Player> datas = new Dictionary<int, Player>();


        public Player AddPlayer(int playerId)
        {
            Player player = new Player();
            player.playerId = playerId;
            datas.Add(playerId, player);
            return player;
        }

        public void RemovePlayer(int playerId)
        {
            if (datas.ContainsKey(playerId))
            {
                datas.Remove(playerId);
            }
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



        public void ClearPlayer()
        {
            foreach (var item in datas)
            {
                item.Value.Clear();
            }
            datas.Clear();
        }
    }
}
