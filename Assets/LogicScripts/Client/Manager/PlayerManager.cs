using Assets.LogicScripts.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Manager
{
    class PlayerManager:Manager<PlayerManager>
    {
        private Player self;
        public Player Self { get => self; set => self = value; }

        public bool SelfIsExist => self != null;

        public void ClearPlayer()
        {
            if (SelfIsExist)
            {
                Self = null;
            }
        }
    }
}
