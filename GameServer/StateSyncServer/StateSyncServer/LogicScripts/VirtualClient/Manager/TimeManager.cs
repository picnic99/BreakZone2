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
            MonoBridge.GetInstance().StartCoroutine(Coroutine());
            base.Init();
        }

        public void AddOnceTimer(object caller, float delayTime, Action call)
        {
            //TimeVO vo = new TimeVO(caller, TimeVO.ONCE, delayTime, 0, call);
            //vos.Add(vo);
            Timer t = new Timer();
            t.Interval = delayTime;
            ElapsedEventHandler func = null; 
            func = (sender, args) =>
            {
                call.Invoke();
            };
            t.Elapsed += func;
            t.Start();
            datas.Add(caller, t);
        }

        public void AddFrameLoopTimer(object caller, float delayTime, float durationTime, Action call, Action endCall)
        {
            TimeVO vo = new TimeVO(caller, TimeVO.FRAME, delayTime, durationTime, call, endCall);
            vos.Add(vo);
        }

        public void AddLoopTimer(object caller, float delayTime, Action call)
        {
            TimeVO vo = new TimeVO(caller, TimeVO.FOREVER, delayTime, float.MaxValue, call);
            vos.Add(vo);
        }


        public void RemoveTimer(object caller, Action call)
        {
            for (int i = 0; i < vos.Count; i++)
            {
                TimeVO vo = vos[i];
                if (vo.caller == caller && vo.call == call)
                {
                    vos.Remove(vo);
                    return;
                }
            }
        }

        public void RemoveAllTimer(object caller)
        {
            List<TimeVO> temp = new List<TimeVO>();
            for (int i = 0; i < vos.Count; i++)
            {
                TimeVO vo = vos[i];
                if (vo.caller == caller)
                {
                    temp.Add(vo);
                }
            }

            foreach (var item in temp)
            {
                vos.Remove(item);
            }
        }

        /// <summary>
        /// 持续时间触发N次
        /// </summary>
        /// <param name="durationTime">持续时间</param>
        /// <param name="triggerCtn">触发次数</param>
        /// <param name="call">触发的函数</param>
        /// <param name="triggerTime">触发时间点 默认为平均时间 time/ctn </param>
        public void AddTimeByDurationCtn(object caller, float durationTime, int triggerCtn, Action call, bool immediateInvoke = false, float[] triggerTime = null)
        {
            List<float> tTimes = new List<float>();
            float cd = durationTime / triggerCtn;
            if (triggerTime == null)
            {
                float offSetTime = immediateInvoke ? 0 : cd;
                for (int i = 0; i < triggerCtn; i++)
                {
                    tTimes.Add(cd * i + offSetTime);
                }
            }
            else
            {
                tTimes = new List<float>(triggerTime);
            }

            foreach (var item in tTimes)
            {
                AddOnceTimer(caller, item, call);
            }
        }

        /// <summary>
        /// 下一帧调用
        /// </summary>
        /// <param name="call"></param>
        public void AddFixedFrameCall(Action call, int frameCtn = 1)
        {
            MonoBridge.GetInstance().StartCoroutine(WaitFixedFrame(call, frameCtn));
        }

        IEnumerator WaitFixedFrame(Action call, int frameCtn)
        {
            for (int i = 0; i < frameCtn; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            call();
        }



        IEnumerator Coroutine()
        {
            while (true)
            {

                for (int i = 0; i < vos.Count; i++)
                {
                    var vo = vos[i];
                    if (vo.delayTime > 0)
                    {
                        vo.delayTime -= Global.FixedFrameTimeMS;
                    }
                    else
                    {
                        vo.call();
                        vo.durationTime -= Global.FixedFrameTimeMS;
                        if (vo.durationTime <= 0)
                        {
                            if (vo.type != TimeVO.FOREVER)
                            {
                                vo.endCall?.Invoke();
                                vos.Remove(vo);
                            }
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}