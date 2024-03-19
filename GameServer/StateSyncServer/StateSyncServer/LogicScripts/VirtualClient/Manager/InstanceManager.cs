using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
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

        public void AddInstance(GameInstance ins, string prefabName, Vector3 pos = default, Vector3 scale = default, float rot = 0)
        {
            ins.InstanceId = CurIntanceId++;
            ins.PrefabName = prefabName;
            if (pos == default)
            {
                pos = Vector3.Zero;
            }
            ins.Trans.Position = pos;
            if (scale == default)
            {
                scale = Vector3.One;
            }
            ins.Trans.Scale = scale;
            ins.Trans.Rot = rot;
            datas.Add(ins.InstanceId, ins);
        }

        public GameInstance CreateEffectInstance(string prefabName, Vector3 pos, float rot)
        {
            GameInstance ins = new GameInstance();
            ins.SetInstanceType(InstanceTypeEnum.EFFECT);
            AddInstance(ins, prefabName, pos, Vector3.One, rot);
            return ins;
        }

        public GameInstance CreateSkillInstance(string prefabName, Vector3 pos, float rot)
        {
            GameInstance ins = new GameInstance();
            ins.SetInstanceType(InstanceTypeEnum.EFFECT);
            AddInstance(ins, prefabName, pos, Vector3.One, rot);
            return ins;
        }

        public GameInstance CreateCrtInstance(string prefabName, Character crt)
        {
            GameInstance ins = new GameInstance();
            ins.SetInstanceType(InstanceTypeEnum.CHARACTER);
            AddInstance(ins, prefabName);
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
