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
        public int playerId;
        public string username;
        public string password;

        public int lastStaySceneId = 1;
        public Vector3 lastStayPos = Vector3.zero;
    }
}
