using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理器
/// 实现基础的监听触发功能
/// </summary>
public class EventDispatcher : Singleton<EventDispatcher>
{
    public static string ADD_RECORD { get { return GetInstance().GetType().Name + "ADD_RECORD"; } }

    public static string MAIN_ROLE_CHANGE { get { return GetInstance().GetType().Name + "MAIN_ROLE_CHANGE"; } }

    public static string PLAYER_JUMPED { get { return GetInstance().GetType().Name + "PLAYER_JUMPED"; } }
    //玩家操作减少状态持续时间
    public static string OPT_REDUCE_STATE_TIME { get { return GetInstance().GetType().Name + "OPT_REDUCE_STATE_TIME"; } }
    //有对象销毁时
    public static string OBJ_DESTROY { get { return GetInstance().GetType().Name + "OBJ_DESTROY"; } }
    //
    public static string SCENE_CHANGE { get { return GetInstance().GetType().Name + "SCENE_CHANGE"; } }

    public static string CHARACTER_DESTORY { get { return GetInstance().GetType().Name + "CHARACTER_DESTORY"; } }
    //数值发生改变
    public static string VALUE_CHANGE { get { return GetInstance().GetType().Name + "VALUE_CHANGE"; } }


    public Dictionary<string, Delegate> events = new Dictionary<string, Delegate>();

    private void AddDelegate(string eventName, Delegate callback)
    {
        Delegate value;
        if (!events.TryGetValue(eventName, out value))
        {
            //key不存在就添加
            events.Add(eventName, callback);
        }
        else
        {
            //key存在判断value是否为空，为空就替换，不为空就多播
            value = (value != null) ? Delegate.Combine(value, callback) : callback;
            events[eventName] = value;
        }
    }

    private void RemoveDelegate(string eventName, Delegate callback)
    {
        Delegate func;
        if (events.TryGetValue(eventName, out func))
        {
            if (func != null)
            {
                func = Delegate.Remove(func, callback);
                events[eventName] = func;
            }
            else
            {
                Debug.LogError("Key:" + eventName + "不存在！");
            }
        }
    }

    public void On(string eventName, Action<object[]> callback)
    {
        AddDelegate(eventName, callback);
    }

    public void Off(string eventName, Action<object[]> callback)
    {
        RemoveDelegate(eventName, callback);
    }

    public void Event(string eventName, params object[] args)
    {
        events.TryGetValue(eventName, out Delegate call);
        if (call != null)
        {
            ((Action<object[]>)call).Invoke(args);
        }

    }
}