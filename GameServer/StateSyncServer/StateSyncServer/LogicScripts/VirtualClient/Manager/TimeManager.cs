using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class TimeVO
    {
        public static int FRAME = 1; //每帧一次
        public static int FOREVER = 2; //每帧一次直到永远调用
        public static int ONCE = 3;//只调一次

        public object caller;
        public int type;
        public float delayTime;
        public float durationTime;
        public Action call;
        public Action endCall;

        public TimeVO(object caller, int type, float delayTime, float durationTime, Action call, Action endCall = null)
        {
            this.caller = caller;
            this.type = type;
            this.delayTime = delayTime;
            this.durationTime = durationTime;
            this.call = call;
            this.endCall = endCall;
        }
    }

    public class TimeManager : Manager<TimeManager>
    {
        public List<TimeVO> vos = new List<TimeVO>();

        public Dictionary<object, List<Timer>> datas = new Dictionary<object, List<Timer>>();

        public override void Init()
        {
            //MonoBridge.GetInstance().StartCoroutine(Coroutine());
            base.Init();
        }

        public void RegisterTimer(object call, Timer t)
        {
            if (t == null) return;

            if (datas.ContainsKey(call))
            {
                List<Timer> timers = datas[call];
                if (timers == null)
                {
                    timers = new List<Timer>();
                }
                t.Start();
                timers.Add(t);
            }
            else
            {
                datas.Add(call, new List<Timer>() { t });
            }
        }

        public void RemoveTimer(object call, Timer t)
        {
            if (t == null) return;
            if (datas.ContainsKey(call))
            {
                List<Timer> timers = datas[call];
                if (timers != null)
                {
                    t.Stop();
                    t.Dispose();
                    timers.Remove(t);
                }
            }
        }

        public Timer AddOnceTimer(object caller, float delayTime, Action call)
        {
            Timer t = new Timer();
            t.Interval = delayTime;
            ElapsedEventHandler func = null;
            func = (sender, args) =>
            {
                call.Invoke();
                RemoveTimer(call, t);
            };
            t.Elapsed += func;
            RegisterTimer(caller, t);
            return t;
        }

        /// <summary>
        /// 添加一个循环计时器
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="call">方法</param>
        /// <param name="delayTime">延迟执行</param>
        /// <param name="fixedTime">固定间隔</param>
        /// <param name="ctn">最大次数 -1 无限大</param>
        /// <returns></returns>
        public Timer AddLoopTimer(object caller, Action call, float delayTime = 0, float fixedTime = Global.FixedFrameTimeS, int ctn = -1)
        {
            int curCtn = 0;
            Timer t = new Timer();
            t.Interval = delayTime;
            t.AutoReset = true;
            ElapsedEventHandler func = null;
            func = (sender, args) =>
            {
                call.Invoke();
                t.Interval = fixedTime;
                curCtn++;
                if (ctn != -1 && curCtn >= ctn)
                {
                    RemoveTimer(call, t);
                }
            };
            t.Elapsed += func;
            RegisterTimer(caller, t);
            return t;
        }

        public void RemoveAllTimer(object caller)
        {
            if (datas.ContainsKey(caller))
            {
                List<Timer> timers = datas[caller];
                if(timers != null)
                {
                    foreach (var item in timers)
                    {
                        item.Stop();
                        item.Dispose();
                    }
                    timers.Clear();
                }
                datas.Remove(caller);
            }
        }
    }
}