using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.VO
{
    public class AnimCoverVO
    {
        public string animName;
        public int sort;

        public AnimCoverVO(string animName, int sort = 0)
        {
            this.animName = animName;
            this.sort = sort;
        }
    }
}
