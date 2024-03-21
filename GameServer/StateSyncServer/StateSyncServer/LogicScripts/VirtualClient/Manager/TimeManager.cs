using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class BaseTimer
    {
        /// <summary>
        /// 定时器类型 1 = 执行一次 2 = 循环执行
        /// </summary>
        public int type;
        /// <summary>
        /// 延迟时间 单位毫秒
        /// </summary>
        public float delayTime;
        /// <summary>
        /// 每次循环间隔  单位毫秒
        /// </summary>
        public float fixedTime;
        /// <summary>
        /// 最大循环执行次数
        /// </summary>
        public int maxLoopNum;
        /// <summary>
        /// 当前循环执行次数
        /// </summary>
        public int curLoopNum;
        /// <summary>
        /// 创建时间
        /// </summary>
        public long createTime;
        /// <summary>
        /// 调用者
        /// </summary>
        public object caller;
        /// <summary>
        /// 调用函数
        /// </summary>
        public Action call;
        /// <summary>
        /// 定时器已结束
        /// </summary>
        public bool IsEnd;
        /// <summary>
        /// 是否首次调用
        /// </summary>
        public bool IsFrist;

        public long lastCallTime;

        public BaseTimer(object caller, Action call, int type = 1, float delayTime = 0, float fixedTime = 1000, int maxLoopNum = -1)
        {
            this.caller = caller;
            this.call = call;
            this.type = type;
            this.delayTime = delayTime;
            this.fixedTime = fixedTime;
            DateTimeOffset now = DateTimeOffset.Now;
            this.createTime = now.Ticks / 10000;
            this.lastCallTime = 0;
            this.maxLoopNum = maxLoopNum;
            this.curLoopNum = 0;
            IsEnd = false;
            IsFrist = true;
        }

        public void Check(long curTime)
        {
            if (IsEnd) return;
            if (IsFrist)
            {
                if (curTime - createTime >= delayTime)
                {
                    call.Invoke();
                    IsFrist = false;
                    lastCallTime = curTime;
                    if (type == 1)
                    {
                        IsEnd = true;
                    }
                    else
                    {
                        curLoopNum++;
                    }
                }
            }
            else
            {
                if (curTime - lastCallTime >= fixedTime)
                {
                    call.Invoke();
                    lastCallTime = curTime;
                    curLoopNum++;
                }
            }

            if (type == 2 && maxLoopNum != -1 && curLoopNum >= maxLoopNum)
            {
                IsEnd = true;
            }
        }

        public bool IsValid()
        {
            return IsEnd == false;
        }

        public void Stop()
        {
            IsEnd = true;
        }
    }


    public class TimeManager : Manager<TimeManager>
    {
        public List<BaseTimer> timerCallList_10ms = new List<BaseTimer>();

        public void Tick(long curTime)
        {
            for (int i = 0; i < timerCallList_10ms.Count; i++)
            {
                var timer = timerCallList_10ms[i];
                timer.Check(curTime);
                if (!timer.IsValid())
                {
                    timerCallList_10ms.Remove(timer);
                }
            }
        }

        private void AddTimer(BaseTimer timer)
        {
            timerCallList_10ms.Add(timer);
        }

        public BaseTimer AddOnceTime(object caller, int delayTime, Action call)
        {
            var timer = new BaseTimer(caller, call, 1, delayTime);
            AddTimer(timer);
            CommonUtils.Logout("添加onceTimer");
            return timer;
        }

        public BaseTimer AddLoopTime(object caller, int delayTime, int fixedTime, Action call, int maxLoopNum = -1)
        {
            var timer = new BaseTimer(caller, call, 2, delayTime, fixedTime, maxLoopNum);
            AddTimer(timer);
            CommonUtils.Logout("添加loopTimer");
            return timer;
        }

        public void ClearTimer(BaseTimer timer)
        {
            timer.Stop();
        }

        public void ClearAllTimer(object caller)
        {
            foreach (var item in timerCallList_10ms)
            {
                if (item.caller == caller)
                {
                    item.Stop();
                }
            }
        }
    }
}