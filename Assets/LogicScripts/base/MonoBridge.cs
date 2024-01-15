using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoBridge : MonoBehaviour
{
    public static MonoBridge instance;

    public Vector3 ve;

    private List<Action> callList = new List<Action>();
    public static MonoBridge GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        if (callList.Count > 0)
        {
            foreach (var call in callList)
            {
                call.Invoke();
            }
        }
    }

    public T CreateOBJ<T>(T perfab) where T : UnityEngine.Object
    {
        return GameObject.Instantiate<T>(perfab);
    }

    public void DestroyOBJ(UnityEngine.Object obj)
    {
        EventDispatcher.GetInstance().Event(EventDispatcher.OBJ_DESTROY, obj);
        GameObject.Destroy(obj);
    }

    public void AddCall(Action call)
    {
        if (!callList.Contains(call))
        {
            callList.Add(call);
        }
    }

    public void RemoveCall(Action call)
    {
        if (callList.Contains(call))
        {
            callList.Remove(call);
        }
    }


    private void OnDrawGizmos()
    {
        if (GameContext.SelfRole == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(GameContext.SelfRole.trans.position, new Vector3(0.3f, 0.1f, 0.3f));
    }
}