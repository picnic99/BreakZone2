using Msg;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    class InputManager
    {
        public Character crt;

        public float thresholdMove = 0f;

        public InputManager(Character crt)
        {
            this.crt = crt;
        }

        public void ApplyOpt(GamePlayerOptReq req)
        {

        }
    }
}
