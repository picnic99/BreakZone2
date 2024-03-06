using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateSyncServer.LogicScripts.VirtualClient.Scene
{
    public class GameScene : Base
    {
        public string SceneName;
        public UIBinding bind;

        /// <summary>
        /// �Ƿ���ս������
        /// </summary>
        public bool IsFightScene = false;

        public List<string> SceneUIs = new List<string>();
        public List<Character> SceneCrts = new List<Character>();

        public virtual void OnEnter()
        {
            bind = GameObject.Find("SceneData")?.GetComponent<UIBinding>();
        }

        public virtual void OnExit()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public void AddCrtToScene()
        {

        }
    }
}