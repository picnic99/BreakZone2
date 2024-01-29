using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager : Manager<ResourceManager>
{
    private float maxLoadTime = 3f;
    public T LoadResource<T>(string name) where T : Object
    {
        return Resources.Load<T>(name);
    }

    public T GetObjInstance<T>(string name) where T : Object
    {
        var obj = LoadResource<T>(name);
        if (obj != null)
        {
            obj = GameObject.Instantiate<T>(obj);
        }
        return obj;
    }

    public T GetObjectInstance<T>(string name) where T : Object
    {
        var obj = LoadResource<T>(name);
        if (obj != null)
        {
            obj = Object.Instantiate(obj);
        }
        return obj;
    }
    public T GetCharacterInstance<T>(string name) where T : Object
    {
        var obj = LoadResource<T>("Character/" + name);
        if (obj != null)
        {
            obj = GameObject.Instantiate<T>(obj);
        }
        return obj;
    }

    public Sprite GetIcon(string path)
    {
       return GetObjectInstance<Sprite>("Icons/" + path);
    }

    public AnimationClip GetAnimatinClip(string name)
    {
        var obj = LoadResource<AnimationClip>("Anims/" + name);
        return obj;
    }

    public AudioClip GetAudioClip(string name)
    {
        var obj = LoadResource<AudioClip>("Sounds/" + name);
        return obj;
    }

    public void LoadResourceAsyn<T>(string name,Action<T> finishCall) where T : Object
    {
        MonoBridge.GetInstance().StartCoroutine(AsynLoad(name, finishCall));
    }

    IEnumerator AsynLoad<T>(string name,Action<T> finishCall) where T : Object
    {
        float tmp_time = maxLoadTime;
        ResourceRequest asset = Resources.LoadAsync<T>(name);
        while (!asset.isDone)
        {
            tmp_time -= Time.deltaTime;
            if (tmp_time < 0)
            {
                Debug.LogError($"{name} 资源加载失败 请检查！");
                break;
            }
            yield return 0;
        }
        if (asset.isDone)
        {
            finishCall((T)asset.asset);
        }
    }
}