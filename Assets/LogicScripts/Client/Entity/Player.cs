using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.Entity
{
    class Player
    {
        public string username;

        public int lastStaySceneId;
        public Vector3 lastStayPos;

        public Player(string username, int lastStaySceneId, Vector3 lastStayPos)
        {
            this.username = username;
            this.lastStaySceneId = lastStaySceneId;
            this.lastStayPos = lastStayPos;
        }
    }
}
