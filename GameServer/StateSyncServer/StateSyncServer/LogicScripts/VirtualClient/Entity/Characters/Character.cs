using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Bridge;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Entity.Characters
{
    public class CharacterBaseInfo
    {
        public bool isDebug = false;
        public bool canControl = false;
        public bool needControl = false;
        public bool isNeedStateBar = false;

        public static CharacterBaseInfo GetShowBaseInfo()
        {
            CharacterBaseInfo info = new CharacterBaseInfo();
            return info;
        }

        public static CharacterBaseInfo GetFightBaseInfo()
        {
            CharacterBaseInfo info = new CharacterBaseInfo();
            info.canControl = true;
            info.needControl = true;
            info.isNeedStateBar = true;
            return info;
        }
    }

    public class Character:EventDispatcher
    {
        public CharacterBaseInfo baseInfo { get; set; }
        //阵营
        public CharacterState state = CharacterState.NEUTRAL;
        //用于控制角色位置
        public Transform trans { get; set; }
        //用于播放角色动画
        public Animator anim { get; set; }
        //记录角色属性数值
        public Property property { get; set; }
        //物理控制器
        public PhysicController physic { get; set; }
        //状态机 只负责角色状态之间的切换
        public FSM fsm { get; set; }

    }
}
