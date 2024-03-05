using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    public class Manager<T> : EventDispatcher where T : new()
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
            AddEventListener();
        }

        public virtual void Destroy()
        {
            RemoveEventListener();
        }

        public virtual void AddEventListener()
        {

        }

        public virtual void RemoveEventListener ()
        {

        }
    }
}
