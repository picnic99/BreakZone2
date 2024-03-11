using Assets.LogicScripts.Client.Entity;
using Assets.LogicScripts.Client.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Common
{
    class Global
    {
        public static bool InGameScene;
        public static int SelfPlayerId;

        public static Player Self => PlayerManager.GetInstance().Self;
    }
}
