using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class GameEventDispatcher:EventDispatcher
    {
        private static GameEventDispatcher _instance;

        public static GameEventDispatcher GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameEventDispatcher();
            }
            return _instance;
        }
    }
}
