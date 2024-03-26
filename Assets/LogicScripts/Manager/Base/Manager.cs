using UnityEngine;

public class Manager<T> : Singleton<T>, IManager where T : new()
{
    private static _EventDispatcher _eventer;
    public static _EventDispatcher Eventer
    {
        get
        {
            if (_eventer == null) _eventer = new _EventDispatcher();
            return _eventer;
        }
    }

    public Manager()
    {
    }

    public virtual void AddEventListener()
    {

    }

    public virtual void RemoveEventListener()
    {

    }

    public virtual void Clear()
    {
        Debug.Log(typeof(T).Name + " 管理器清理");
        RemoveEventListener();
    }

    public virtual void Init()
    {
        Debug.Log(typeof(T).Name + " 管理器初始化");
        AddEventListener();
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnLaterUpdate()
    {

    }
}