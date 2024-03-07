using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    class InstanceManager : Manager<InstanceManager>
    {
        public int CurIntanceId = 1;

        public Dictionary<int, GameInstance> datas = new Dictionary<int, GameInstance>();

        public void AddInstance(GameInstance ins)
        {
            ins.InstanceId = CurIntanceId++;
            datas.Add(ins.InstanceId, ins);
        }

        public GameInstance CreateEffectInstance(string prefabName,Vector3 pos,float rot)
        {
            GameInstance ins = new GameInstance(null);
            ins.SetInstanceType(InstanceTypeEnum.EFFECT);
            ins.PrefabName = prefabName;
            ins.trans.Position = pos;
            ins.trans.Rot = rot;
            AddInstance(ins);
            return ins;
        }

        public GameInstance CreateSkillInstance(string prefabName, Vector3 pos, float rot)
        {
            GameInstance ins = new GameInstance(null);
            ins.SetInstanceType(InstanceTypeEnum.EFFECT);
            ins.PrefabName = prefabName;
            ins.trans.Position = pos;
            ins.trans.Rot = rot;
            AddInstance(ins);
            return ins;
        }

        public GameInstance FindInstance(int instanceId)
        {
            if (datas.ContainsKey(instanceId))
            {
                return datas[instanceId];
            }
            return null;
        }

        public bool RemoveInstance(GameInstance instance)
        {
            int key = 0;
            foreach (var item in datas)
            {
                if (item.Value == instance)
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

        /// <summary>
        /// 发送创建实例协议
        /// </summary>
        public void SendInstanceCreateProto()
        {

        }
        /// <summary>
        /// 发送实例变换协议
        /// </summary>
        public void SendTransformProto()
        {

        }
    }
}
