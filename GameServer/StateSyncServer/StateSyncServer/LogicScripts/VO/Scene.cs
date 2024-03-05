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
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName;
        /// <summary>
        /// 场景尺寸 默认 100M * 100M
        /// </summary>
        public Vector2 SceneSize = new Vector2(100, 100);
    }
}
