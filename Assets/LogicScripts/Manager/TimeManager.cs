using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    public TimeVO(object caller, int type, float delayTime, float durationTime, Action call,Action endCall = null)
    {
        this.caller = caller;
        this.type = type;
        this.delayTime = delayTime;
        this.durationTime = durationTime;
        this.call = call;
        this.endCall = endCall;
    }
}

public class TimeManager:Singleton<TimeManager>,Manager
{
    public List<TimeVO> vos = new List<TimeVO>();

    public void Init()
    {
        MonoBridge.GetInstance().StartCoroutine(Coroutine());
    }

    public void AddOnceTimer(object caller,float delayTime,Action call)
    {
       TimeVO vo = new TimeVO(caller,TimeVO.ONCE, delayTime,0, call);
        vos.Add(vo);
    }

    public void AddFrameLoopTimer(object caller,float delayTime,float durationTime, Action call,Action endCall)
    {
        TimeVO vo = new TimeVO(caller,TimeVO.FRAME, delayTime, durationTime, call, endCall);
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
            if(vo.caller == caller && vo.call == call)
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


    IEnumerator Coroutine()
    {
        while (true)
        {

            for (int i = 0; i < vos.Count; i++)
            {
                var vo = vos[i];
                if (vo.delayTime > 0)
                {
                    vo.delayTime -= Time.deltaTime;
                }
                else
                {
                    vo.call();
                    vo.durationTime -= Time.deltaTime;
                    if (vo.durationTime <= 0)
                    {
                        if(vo.type != TimeVO.FOREVER)
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