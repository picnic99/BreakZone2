using System;
using UnityEngine;

/// <summary>
/// 特效管理器
/// 负责特效的创建与回收
/// 如施法技能时附带的特效 角色身上的特效 各种特效
/// </summary>
public class EffectManager:Manager<EffectManager>
{
    public GameObject PlayEffect(string name, float durationTime, Transform parant, Vector3 pos, Vector3 dir, Vector3 scale)
    {
        GameObject asset = ResourceManager.GetInstance().LoadResource<GameObject>("Effect/" + name);
        if (asset)
        {
            GameObject obj = GameObject.Instantiate<GameObject>(asset);
            if (parant != null) obj.transform.SetParent(parant);
            obj.transform.position = pos;
            obj.transform.forward = dir;
            obj.transform.localScale = scale;
            TimeManager.GetInstance().AddOnceTimer(this, durationTime, () =>
            {
                GameObject.Destroy(obj);
            });
            return obj;
        }
        return null;
    }
}