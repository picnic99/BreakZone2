using Assets.LogicScripts.Client.Common;
using Assets.LogicScripts.Client.Entity;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Manager
{
    class SceneManager:Manager<SceneManager>
    {
        public static string ENTER_SCENE_SUCCESS = "ENTER_SCENE_SUCCESS";

        public Dictionary<int, Player> scenePlayerInfos = new Dictionary<int, Player>();

        public override void AddEventListener()
        {
            base.AddEventListener();
            On(ENTER_SCENE_SUCCESS, InitScene);        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            Off(ENTER_SCENE_SUCCESS, InitScene);
        }

        public void InitScene(object[] args)
        {
            if (Global.InGameScene)
            {
                if(scenePlayerInfos.Count > 0)
                {

                }
            }
        }

        public void UpdateScene(object[] args)
        {
/*            GamePlayerBaseInfo info = args[0] as GamePlayerBaseInfo;
            if (scenePlayerInfos.ContainsKey(info.PlayerId))
            {
                var p = scenePlayerInfos[info.PlayerId];
                if (p != null)
                {
                    p.UpdateBaseInfo(info);
                }
            }
            else
            {
                PlayerManager.GetInstance().ge
            }*/
        }

        public void AddPlayerToScene(GamePlayerBaseInfo info)
        {
            Player p = null;
            if (scenePlayerInfos.ContainsKey(info.PlayerId))
            {
                p = scenePlayerInfos[info.PlayerId];
            }
            else
            {
                if(info.PlayerId == Global.SelfPlayerId)
                {
                    p = PlayerManager.GetInstance().Self;
                }
                else
                {
                    p = PlayerManager.GetInstance().AddPlayer(info.PlayerId);
                }
                scenePlayerInfos.Add(p.playerId, p);
            }

            if (p != null)
            {
                p.UpdateBaseInfo(info);
            }
        }
    }
}
