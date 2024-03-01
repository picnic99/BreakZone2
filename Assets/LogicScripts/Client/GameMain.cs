using Assets.LogicScripts.Client.Net.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client
{
    class GameMain
    {
        private static GameMain _instance;

        public static GameMain GetInstance()
        {
            if (_instance == null) _instance = new GameMain();
            return _instance;
        }

        public Queue<Protocol> ProtoList = new Queue<Protocol>();


        public void OnStart()
        {

        }

        public void OnUpdate()
        {
            //协议处理
            while (ProtoList.Count > 0)
            {
                ActionManager.GetInstance().Handle(ProtoList.Dequeue());
            }
            InputManager.GetInstance().OnUpdate();

        }

        public void OnDestroy()
        {

        }
    }
}
