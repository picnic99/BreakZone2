using System;
using System.Collections;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

public class ResourceManager : Manager<ResourceManager>
{
    public static string RESOURCE_PREFIX = "Res/";
    public static string AB_PREFIX = "Assets/Res/";

    private static bool ResourceFromAB = GameContext.ResourceFromAB;

    private float maxLoadTime = 3f;
    public T LoadResource<T>(string name) where T : Object
    {
        T result;
        if (ResourceFromAB)
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            var temp = package.LoadAssetSync<T>(AB_PREFIX + name);
            result = (T)temp.AssetObject;
        }
        else
        {
            result = Resources.Load<T>(RESOURCE_PREFIX + name);
        }
        return result;
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
            obj = GameObject.Instantiate(obj);
        }
        return obj;
    }
    public T GetCharacterInstance<T>(string name) where T : Object
    {
        var obj = LoadResource<T>("prefabs/Character/" + name);
        if (obj != null)
        {
            obj = GameObject.Instantiate<T>(obj);
        }
        return obj;
    }

    public GameObject GetSkillInstance(string name)
    {
        var obj = LoadResource<GameObject>("prefabs/Skill/" + name);
        if (obj != null)
        {
            obj = GameObject.Instantiate(obj);
        }
        return obj;
    }

    public GameObject GetEffectInstance(string name)
    {
        var obj = LoadResource<GameObject>("prefabs/Effect/" + name);
        if (obj != null)
        {
            obj = GameObject.Instantiate(obj);
        }
        return obj;
    }

    public Sprite GetIcon(string path)
    {
       return GetObjectInstance<Sprite>("picture/Icons/" + path);
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

    public TextAsset GetConfigRes(string name)
    {

        var obj = LoadResource<TextAsset>("config/" + name);
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