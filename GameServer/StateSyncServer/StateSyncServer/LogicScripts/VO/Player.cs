using Msg;
using StateSyncServer.LogicScripts.Manager;
using System.Collections;
using System.Collections.Generic;
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
        public int lastStaySceneId = 1;
        public int SceneId => lastStaySceneId;
        public int lastSelectCrtId = 1;
        public int CrtId => lastSelectCrtId;
        public Vector3 lastStayPos = Vector3.Zero;

        public Vector2 input = Vector2.Zero;

        public VirtualClient.Characters.Character Crt => CharacterManager.GetInstance().FindCharacter(playerId);

        public Player()
        {
        }

        public void SetInput(GamePlayerInputCmdReq input)
        {
            CharacterManager.GetInstance().FindCharacter(playerId).ApplyInput(input);
        }

        public void SetOpt(GamePlayerOptCmdReq opt)
        {
            CharacterManager.GetInstance().FindCharacter(playerId).ApplyOpt(opt);
        }
    }
}
