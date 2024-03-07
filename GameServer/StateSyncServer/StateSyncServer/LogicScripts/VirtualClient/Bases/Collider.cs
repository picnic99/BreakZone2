using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    public class Collider
    {
        public int ColliderCheckType = InstanceTypeEnum.ALL;
        /// <summary>
        /// 有物体进入范围时调用
        /// </summary>
        public Action<Character[]> enterCall { get; set; }
        /// <summary>
        /// 有物体停留时调用
        /// </summary>
        public Action<Character[]> stayCall { get; set; }
        /// <summary>
        /// 有物体离开范围时调用
        /// </summary>
        public Action<Character[]> exitCall { get; set; }

        public virtual void Check()
        {

        }
    }
}
