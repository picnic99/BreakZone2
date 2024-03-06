using Msg;
using StateSyncServer.LogicScripts.Manager;
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
        public int playerId;
        public string username;
        public string password;

        public int lastStaySceneId = 1;
        public int SceneId => lastStaySceneId;
        public int lastSelectCrtId = 1;
        public Vector3 lastStayPos = Vector3.Zero;

        public Vector2 input = Vector2.Zero;

        public Player(int playerId, string username, string password)
        {
            this.playerId = playerId;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// 存储玩家的输入数据
        /// </summary>
        public void SetInput(Vector2 input)
        {
            this.input = input;
        }

        public void ApplyPlayerOpt(GamePlayerOptReq opt)
        {
            //将玩家的输入告知Crt  crt根据input的数据 去分析是否需要改变状态 然后流程可以走原先的一套
            CharacterManager.GetInstance().Event(CharacterManager.PLAYER_OPT_CHANGE, playerId, opt);
        }
    }
}
