using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    public class Instance
    {
        protected int _playerId;
        public int PlayerId => _playerId;
        public Player Player => PlayerManager.GetInstance().FindPlayer(_playerId);
    }
}
