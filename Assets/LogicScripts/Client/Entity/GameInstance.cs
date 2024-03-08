using Assets.LogicScripts.Client.Manager;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.Entity
{
    public class GameInstance
    {
        public int instanceId => data.InstanceId;

        public GameObject obj;

        public GameInstanceData data;

        public GameInstance(GameInstanceData data)
        {
            this.data = data;
        }

        public void ApplyCrtData()
        {
            if(obj == null)
            {
                obj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>("SwordWoman");
                if(instanceId == PlayerManager.GetInstance().Self.instanceId)
                {
                    EventDispatcher.GetInstance().Event(EventDispatcher.MAIN_ROLE_CHANGE, this);
                }
            }
            obj.transform.position = new Vector3(data.Pos.X,data.Pos.Y,data.Pos.Z);
            obj.transform.rotation = Quaternion.Euler(0, data.Rot, 0);
        }


    }
}
