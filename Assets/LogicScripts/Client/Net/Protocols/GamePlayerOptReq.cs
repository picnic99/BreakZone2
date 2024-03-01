
using Assets.LogicScripts.Client.Enum;
using UnityEngine;

namespace Assets.LogicScripts.Client.Net.Protocols
{
    class GamePlayerOptReq : Protocol
    {
        public Vector3 input;
        public float Rot;
        public PlayerOptEnum UpMove;//前进
        public PlayerOptEnum DownMove;//后退
        public PlayerOptEnum LeftMove;//左移
        public PlayerOptEnum RightMove;//右移
        public PlayerOptEnum AddSpeed;//右移
        public PlayerOptEnum Arm;//瞄准
        public PlayerOptEnum Flash;//闪避
        public PlayerOptEnum Jump;//跳跃
        public PlayerOptEnum Atk;//攻击
        public PlayerOptEnum Skill1;//技能1
        public PlayerOptEnum Skill2;//技能2
        public PlayerOptEnum Skill3;//技能3
        public PlayerOptEnum Skill4;//技能4

        public GamePlayerOptReq()
        {
            protocolId = ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ;

        }
    }
}
