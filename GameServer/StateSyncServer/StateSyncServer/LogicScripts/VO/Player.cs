using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VO
{
    class Player
    {
        public int playerId;
        public string username;
        public string password;

        public int lastStaySceneId = 1;
        public int lastSelectCrtId = 1;
        public Vector3 lastStayPos = Vector3.Zero;

        public Player(int playerId, string username, string password)
        {
            this.playerId = playerId;
            this.username = username;
            this.password = password;
        }
    }
}
