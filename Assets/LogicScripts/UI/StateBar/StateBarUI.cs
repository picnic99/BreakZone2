using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.UI.StateBar
{
    class StateBarUI : UIBase
    {
        public static string UPDATE_STATE_BAR = "UPDATE_STATE_BAR";

        public Dictionary<int, StateBarItem> stateBarDic = new Dictionary<int, StateBarItem>();

        public StateBarUI()
        {
            uiPath = RegPrefabs.StateBarUI;
            belongScene = RegSceneClass.GameRoomScene;
            layer = UILayers.BOTTOM;
        }

        public override void OnLoad()
        {
            base.OnLoad();

        }

        public override void AddEventListener()
        {
            base.AddEventListener();
            UIManager.Eventer.On(StateBarUI.UPDATE_STATE_BAR, UpdateStateBar);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            UIManager.Eventer.Off(StateBarUI.UPDATE_STATE_BAR, UpdateStateBar);

        }

        private void UpdateStateBar(object[] args)
        {
            int playerId = (int)args[0];
            GamePlayerProperty property = args[1] as GamePlayerProperty;
            StateBarItem item = null;
            if (stateBarDic.ContainsKey(playerId))
            {
                item = stateBarDic[playerId];
            }
            else
            {
                item = new StateBarItem(Root.transform, playerId);
                stateBarDic.Add(playerId, item);
            }

            item.UpdateData(property);

        }

        public override void OnLaterUpdate()
        {
            base.OnLaterUpdate();
            foreach (var item in stateBarDic)
            {
                item.Value.OnUpdate();
            }
        }

    }
}
