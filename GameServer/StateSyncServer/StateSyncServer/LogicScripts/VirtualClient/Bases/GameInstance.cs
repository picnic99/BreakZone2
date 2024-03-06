using StateSyncServer.LogicScripts.VirtualClient.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    /// <summary>
    /// 对应游戏中的一个实例
    /// </summary>
    public class GameInstance
    {
        public int InstanceId { get; set; }
        //用于控制角色位置
        public Transform trans { get; set; }
        //用于播放角色动画
        public Animator anim { get; set; }
    }
}
