using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        /// <summary>
        /// sceneId - Scene
        /// </summary>
        public Dictionary<int, Scene> datas = new Dictionary<int, Scene>();

        public Scene GetScene(int sceneId)
        {
            if (datas.ContainsKey(sceneId))
            {
                return datas[sceneId];
            }
            else
            {
                Scene scene = new Scene();
                //以下数据根据ID从数据库查询
                scene.sceneId = sceneId;
                scene.SceneName = "游戏大厅";
                scene.SceneSize = new Vector2(100,100);
                scene.SceneData = null;
                datas.Add(scene.sceneId, scene);
                return scene;
            }
        }

        public void RemoveScene(int sceneId)
        {
            if (datas.ContainsKey(sceneId))
            {
                datas.Remove(sceneId);
            }
        }


        /// <summary>
        /// 将玩家添加到场景中
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="sceneId"></param>
        public void AddPlayerToScene(Player p, int sceneId)
        {
            //玩家加入新场景 创建对应角色到场景中
            Scene scene = GetScene(sceneId);
            scene.AddPlayer(p.playerId);
            p.lastStaySceneId = sceneId;
            //CharacterManager.GetInstance().Event(CharacterManager.PLAYER_ADD_TO_SCENE, playerId);
            CharacterManager.GetInstance().OnPlayerAddToScene(p.playerId);
        }

        /// <summary>
        /// 将玩家移出当前场景
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="sceneId"></param>
        public void RemovePlayerInScene(int playerId)
        {
            Player player = PlayerManager.GetInstance().FindPlayer(playerId);
            Scene scene = GetScene(player.SceneId);
            scene.RemovePlayer(playerId);
            player.lastStaySceneId = 0;
        }

        /// <summary>
        /// 获取某场景中的所有玩家
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public List<Player> GetPlayersInScene(int sceneId)
        {
            return PlayerManager.GetInstance().GetAllPlayerInScene(sceneId);
        }

        /// <summary>
        /// 获取玩家所在场景中的所有玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public List<Player> GetPlayerInSceneByPid(int playerId) 
        {
            Player p = PlayerManager.GetInstance().FindPlayer(playerId);
            if(p != null)
            {
                return GetPlayersInScene(p.SceneId);
            }
            return null;
        }

        /// <summary>
        /// 获取玩家所在场景中的其它玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public List<Player> GetPlayerInSceneByPidNoSelf(int playerId)
        {
            Player p = PlayerManager.GetInstance().FindPlayer(playerId);
            if (p != null)
            {
                return PlayerManager.GetInstance().GetAllPlayerInSceneNoSelf(p.SceneId, playerId);
            }
            return null;
        }

        public void Tick()
        {

        }
    }
}
