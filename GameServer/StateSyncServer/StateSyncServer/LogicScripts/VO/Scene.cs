using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VO
{
    class Scene
    {
        public int sceneId;
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName;
        /// <summary>
        /// 场景内的玩家ID列表
        /// </summary>
        public List<int> playerIds = new List<int>();
        /// <summary>
        /// 场景尺寸 默认 100M * 100M
        /// </summary>
        public Vector2 SceneSize = new Vector2(100, 100);
        /// <summary>
        /// 场景数据
        /// 主要记录场景3D数据 获取某位置是否合法？
        /// </summary>
        public object SceneData;

        /// <summary>
        /// 位置在场景中是否合法？
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsPosLegalInScene(Vector3 pos)
        {
            return true;
        }

        public void AddPlayer(int pId)
        {
            if (!playerIds.Contains(pId))
            {
                playerIds.Add(pId);
            }
        }

        public void RemovePlayer(int pId)
        {
            if (playerIds.Contains(pId))
            {
                playerIds.Remove(pId);
            }
        }
    }
}
