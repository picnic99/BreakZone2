using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterBase<T,K,V> where T: RegisterBase<T, K, V>, new()
{
    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = new T();
            _instance.Init();
        }
        return _instance;
    }

    protected Dictionary<K, V> regDic;

    public virtual void Init() {
        regDic = new Dictionary<K, V>();
    }

    public V Get(K key)
    {
        if (regDic.ContainsKey(key))
        {
            return regDic[key];
        }

        return default(V);
    }
}
