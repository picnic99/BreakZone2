using Assets.LogicScripts.Client.Entity;
using Assets.LogicScripts.Client.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.UI.StateBar
{
    class StateBarItem : UIItemBase
    {
        public int playerId;
        public Character Crt => PlayerManager.GetInstance().FindPlayer(playerId).Crt;

        public StateBarItem()
        {
            GameObject obj = ResourceManager.GetInstance().GetObjInstance<GameObject>("prefabs/UI/" + RegPrefabs.StateBarItem);
            Root = obj;
        }

        public void OnUpdate()
        {
            //更新位置
        }

        public void UpdateData()
        {
            //更新数据
        }

    }
}
