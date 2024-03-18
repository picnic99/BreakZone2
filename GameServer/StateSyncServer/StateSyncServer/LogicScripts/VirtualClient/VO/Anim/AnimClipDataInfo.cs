using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.VO.Anim
{
    [Serializable]
    public class AnimClipDataInfo
    {
        public string animName;
        public float time;
        public List<Vector3> curves;
    }
}
