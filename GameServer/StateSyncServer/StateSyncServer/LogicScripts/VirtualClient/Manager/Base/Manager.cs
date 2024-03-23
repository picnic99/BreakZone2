using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager.Base
{
    public class Manager<T> : VirtualClient.Bases.Singleton<T>, IManager where T : new()
    {
        private static _EventDispatcher _eventer;
        public static _EventDispatcher Eventer
        {
            get
            {
                if (_eventer == null) _eventer = new _EventDispatcher();
                return _eventer;
            }
        }

        public Manager()
        {
        }

        public virtual void AddEventListener()
        {

        }

        public virtual void RemoveEventListener()
        {

        }

        public virtual void Clear()
        {
            CommonUtils.Logout(typeof(T).Name + " 管理器清理");
            RemoveEventListener();
        }

        public virtual void Init()
        {
            CommonUtils.Logout(typeof(T).Name + " 管理器初始化");
            AddEventListener();
        }

        public virtual void OnUpdate()
        {

        }
    }
}