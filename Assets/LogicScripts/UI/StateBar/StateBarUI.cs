using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.UI.StateBar
{
    class StateBarUI: UIBase
    {
        public static string ADD_STATE_BAR = "ADD_STATE_BAR";

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
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
        }

    }
}
