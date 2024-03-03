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
        public string username;
        public string password;

        public int lastStaySceneId;
        public Vector3 lastStayPos;

        public Player(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
