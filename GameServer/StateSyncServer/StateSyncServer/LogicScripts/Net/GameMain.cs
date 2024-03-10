
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Net.PB;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VO;

namespace StateSyncServer.LogicScripts.Net
{
    class GameMain
    {
        private static GameMain _instance;

        public static GameMain GetInstance()
        {
            if (_instance == null) _instance = new GameMain();
            return _instance;
        }

        //线程安全的队列 ConcurrentQueue
        public Queue<Protocol> ProtoList = new Queue<Protocol>();

        public void Run()
        {
            Init();
            long lastTimestamp = 0;
            while (true)
            {
                DateTimeOffset now = DateTimeOffset.Now;
                long curStamp = now.Ticks / 10000;
                if (curStamp - lastTimestamp < Global.FixedFrameTimeMS)
                {
                    continue;
                }
/*                if (watch.IsRunning)
                {
                    watch.Stop();
                    // 获取经过的时间（毫秒）  
                    long elapsedMilliseconds = watch.ElapsedMilliseconds;
                    // 输出结果  
                    Console.WriteLine("执行时间: {0} 毫秒", elapsedMilliseconds);
                    watch.Reset();
                }
                else
                {
                    watch.Start();
                }*/

                lastTimestamp = curStamp;
                //协议处理
                while (ProtoList.Count > 0)
                {
                    Handle(ProtoList.Dequeue());
                }
                Update(null);
            }

        }

        private void Init()
        {
            LogicScripts.Manager.ActionManager.GetInstance().Init();
            LogicScripts.Manager.CharacterManager.GetInstance().Init();
        }


        private Stopwatch watch = new Stopwatch();

        public void Update(object state)
        {
            LogicScripts.Manager.CharacterManager.GetInstance().Tick();
        }

        public void Handle(Protocol protocol)
        {
            LogicScripts.Manager.ActionManager.GetInstance().Event(protocol.protocolId,protocol);
        }
    }
}
