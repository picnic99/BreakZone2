using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class Manager<T> : EventDispatcher where T : new()
    {
        public static T _instance;

        public static T GetInstance()
        {
            if (_instance == null) { 
                _instance = new T();
            } 
            return _instance;
        }

        public virtual void Init()
        {
            AddListener();
        }

        public virtual void Destroy()
        {
            RemoveListener();
        }

        public virtual void AddListener()
        {

        }

        public virtual void RemoveListener ()
        {

        }
    }
}
