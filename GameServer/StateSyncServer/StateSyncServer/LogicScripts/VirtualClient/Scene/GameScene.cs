

using StateSyncServer.LogicScripts.VirtualClient.Bases;

namespace StateSyncServer.LogicScripts.VirtualClient.Scene
{
    public class GameScene : Base
    {
        public string SceneName;

        /// <summary>
        /// 是否是战斗场景
        /// </summary>
        public bool IsFightScene = false;

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {

        }

        public virtual void OnUpdate()
        {

        }
    }
}