using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjPool<T>
{
    public int maxNum = 100;
    public Queue<T> objs = new Queue<T>();

    public T Get()
    {
        T obj;
        if (objs.Count <= 0)
        {
            obj = InstanceObj();
        }
        else
        {
            obj = default;
            while (objs.Count > 0)
            {
                obj = objs.Dequeue();
                if (obj == null)
                {
                    continue;
                }
            }
        }
        return obj;
    }

    public void Recover(T obj)
    {
        if (obj == null) return;
        if (objs.Count >= maxNum)
        {
            return;
        }
        Init(obj);
        objs.Enqueue(obj);
    }

    public abstract T InstanceObj();

    public abstract void Init(T obj);
}