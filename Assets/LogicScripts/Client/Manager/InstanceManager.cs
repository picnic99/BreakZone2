using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.Manager
{
    class InstanceManager: Manager<InstanceManager>
    {
        public static string INSTANCE_CREATE = "INSTANCE_CREATE";

        public override void AddEventListener()
        {
            base.AddEventListener();
            On(INSTANCE_CREATE, OnInstanceCreate);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            Off(INSTANCE_CREATE, OnInstanceCreate);

        }

        private void OnInstanceCreate(object[] args)
        {
            string prefabKey = (string)args[0];
            int stageNum = (int)args[1];
            Vector3 pos = (Vector3)args[2];
            float rot = (float)args[3];

            var obj = ResourceManager.GetInstance().GetSkillInstance(prefabKey);
            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(0, rot, 0);
            obj.transform.Find(stageNum + "").gameObject.SetActive(true);
        }
    }
}
