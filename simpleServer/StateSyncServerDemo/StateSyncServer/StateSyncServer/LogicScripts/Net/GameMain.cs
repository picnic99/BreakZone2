
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Net.Protocols;
using StateSyncServer.LogicScripts.Util;
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

        private float lastTime = 0;

        public Queue<Protocol> ProtoList = new Queue<Protocol>();

        public void Run()
        {
            /*            // 设置定时器回调  
                        TimerCallback timerCallback = new TimerCallback(Update);

                        // 创建一个周期为1000毫秒的定时器  
                        Timer timer = new Timer(timerCallback, null, 0, Global.FixedFrameTimeMS);*/

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


        private Stopwatch watch = new Stopwatch();

        public void Update(object state)
        {
/*            if (watch.IsRunning)
            {
                watch.Stop();
                // 获取经过的时间（毫秒）  
                long elapsedMilliseconds = watch.ElapsedMilliseconds;

                // 输出结果  
                //Console.WriteLine("执行时间: {0} 毫秒", elapsedMilliseconds);
                watch.Reset();
            }
            else
            {
                watch.Start();
            }*/

            //处理事件
/*            for (int i = 0; i < ActionManager.GetInstance().ActionList.Count; i++)
            {
                var action = ActionManager.GetInstance().ActionList[i];
                ActionManager.GetInstance().FightHandle(action);
            }*/
            ActionManager.GetInstance().ClearAction();
            //技能数据迭代
            SkillManager.GetInstance().Tick();
            //数据迭代
            PlayerManager.GetInstance().Tick();
            //弹道迭代
            BulletManager.GetInstance().Tick();
        }

        public void Handle(Protocol protocol)
        {
            bool isFightProto = protocol is PlayerOptReq;

            if (isFightProto)
            {
                //ActionManager.GetInstance().AddAction((PlayerOptReq)protocol);
                ActionManager.GetInstance().FightHandle((PlayerOptReq)protocol);
            }
            else
            {
                ActionManager.GetInstance().LogicHandle(protocol.client, protocol);
            }
        }
    }
}
