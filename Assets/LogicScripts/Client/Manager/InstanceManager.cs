using Assets.LogicScripts.Client.Entity;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Manager
{
    class InstanceManager:Manager<InstanceManager>
    {
        /// <summary>
        /// instanceId - GameInstance
        /// </summary>
        public Dictionary<int, GameInstance> datas = new Dictionary<int, GameInstance>();

        public GameInstance CreateInstance(GameInstanceData data)
        {
            var c = new GameInstance(data);
            datas.Add(data.InstanceId, c);
            return c;
        }

        public GameInstance FindInstance(int instanceId)
        {
            if (datas.ContainsKey(instanceId))
            {
                return datas[instanceId];
            }
            return null;
        }

        public bool RemoveInstance(GameInstance ins)
        {
            int key = 0;
            foreach (var item in datas)
            {
                if (item.Value == ins)
                {
                    key = item.Key;
                    break;
                }
            }
            if (key > 0)
            {
                datas.Remove(key);
                return true;
            }
            return false;
        }

        public void HideAllInstance()
        {
/*            foreach (var item in datas)
            {
                item.Value.SetActive(false);
            }*/
        }
    }
}
