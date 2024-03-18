using Msg;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VO
{
    /// <summary>
    /// 玩家
    /// 负责收集调度客户端的操作
    /// 告知其所管理的Crt
    /// </summary>
    public class Player
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        public int playerId { get; set; }
        /// <summary>
        /// 上次停留的场景
        /// </summary>
        public int lastStaySceneId = 1;
        public int SceneId => lastStaySceneId;
        /// <summary>
        /// 上次旋转的角色
        /// </summary>
        public int lastSelectCrtId = 1;
        public int CrtId => lastSelectCrtId;
        /// <summary>
        /// 上次停留的位置
        /// </summary>
        private Vector3 _lastStayPos = Vector3.Zero;
        public Vector3 LastStayPos { get => _lastStayPos; set => _lastStayPos = value; }
        /// <summary>
        /// 玩家当前的输入
        /// </summary>
        public Vector2 _input = Vector2.Zero;
        public Vector2 Input { get => _input; set => _input = value; }
        /// <summary>
        /// 获取当前角色
        /// </summary>
        public Character Crt => CharacterManager.GetInstance().FindCharacter(playerId);

        public Player()
        {
        }

        public void SetInput(GamePlayerInputCmdReq input)
        {
            _input = new Vector2(input.InputX, input.InputY);
            CharacterManager.GetInstance().FindCharacter(playerId).ApplyInput(input);
        }

        public void SetOpt(GamePlayerOptCmdReq opt)
        {
            CharacterManager.GetInstance().FindCharacter(playerId).ApplyOpt(opt);
        }

        public void SyncState()
        {
            //获得角色数据

            //发送AOI玩家包括自己
        }
    }
}
