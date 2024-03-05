using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{

    class SceneEnum
    {
        public static int GAME_ROOM = 1;
    }
    class SceneManager : Manager<SceneManager>
    {
        public static int DefaultSceneId = SceneEnum.GAME_ROOM;

        public Dictionary<int, Scene> datas = new Dictionary<int, Scene>();

        /// <summary>
        /// 将玩家添加到场景中
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="sceneId"></param>
        public void AddPlayerToScene(int playerId, int sceneId)
        {
            Player player = PlayerManager.GetInstance().FindPlayer(playerId);
            if (player == null) return;
            player.lastStaySceneId = sceneId;
            //玩家加入新场景 创建对应角色到场景中
            CharacterManager.GetInstance().Event(CharacterManager.PLAYER_ADD_TO_SCENE, playerId);
        }

        /// <summary>
        /// 将玩家移出当前场景
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="sceneId"></param>
        public void RemovePlayerInScene(int playerId)
        {
            AddPlayerToScene(playerId, DefaultSceneId);
        }

        public List<Player> GetPlayersInScene(int sceneId)
        {
            return PlayerManager.GetInstance().GetAllPlayerInScene(sceneId);
        }

        public void Tick()
        {

        }
    }
}
