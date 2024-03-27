using Assets.LogicScripts.Client.Entity;
using Assets.LogicScripts.Client.Manager;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LogicScripts.UI.StateBar
{
    class StateBarItem : UIItemBase
    {
        public int playerId;
        public Character Crt => PlayerManager.GetInstance().FindPlayer(playerId).Crt;
        public TextMeshProUGUI nicknameTxt => UIBase.GetBind<TextMeshProUGUI>(Root, "nicknameTxt"); 
        public TextMeshProUGUI stateTxt => UIBase.GetBind<TextMeshProUGUI>(Root, "stateTxt"); 
        public RectMask2D progress => UIBase.GetBind<GameObject>(Root, "progress").GetComponent<RectMask2D>(); 
        public Image progColor => UIBase.GetBind<Image>(Root, "progColor");

        public GamePlayerProperty lastProperty;

        public StateBarItem(Transform parent,int playerId)
        {
            GameObject obj = ResourceManager.GetInstance().GetObjInstance<GameObject>("prefabs/UI/" + RegPrefabs.StateBarItem);
            Root = obj;
            obj.transform.SetParent(parent);
            this.playerId = playerId;
        }

        public void OnUpdate()
        {
            //更新位置
            if (playerId == 0 || Crt == null) return;

            RootRect.anchoredPosition = CameraManager.GetInstance().CamRoot.WorldToScreenPoint(Crt.CrtObj.transform.position + new Vector3(0,1.8f,0));
        }

        public void UpdateData(GamePlayerProperty property)
        {
            if(lastProperty != null)
            {
                if(lastProperty.CurHp != property.CurHp)
                {
                    new PopValue(Root, Crt.CrtObj.transform, property.CurHp - lastProperty.CurHp);
                }
            }
            lastProperty = property;
            //更新数据
            nicknameTxt.text = "破破破域域域";
            stateTxt.text = "无敌";
            SetHp(property.MaxHp,property.CurHp);
        }

        public void SetHp(int maxHp,int curHp)
        {
            progColor.fillAmount = curHp / (float)maxHp;
        }

    }
}
