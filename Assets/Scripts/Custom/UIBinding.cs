using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Bind{
    public string name;
    public GameObject obj;
    public string className;
    public bool isSelect;
}

public struct BindPackage
{
    public UnityEngine.Object obj;

    public T AS<T>() where T : UnityEngine.Object
    {
        return (T)obj;
    }
}

[System.Serializable]
public class UIBinding : MonoBehaviour
{
    public BindPackage this[string name]
    {
        get
        {
           return GetItemPkg(name);
        }
    }

    //[SerializeField]
    [HideInInspector]
    public List<Bind> bindList = new List<Bind>();

    public UnityEngine.Object GetItem(string name)
    {
        foreach (var item in bindList)
        {
            if (item.name == name)
            {
                UnityEngine.Object o = null;
                if (item.className.Equals("UnityEngine.GameObject")) o = (UnityEngine.Object)item.obj;
                else o = item.obj.GetComponent(item.className);
                return o;
            }
        }

        return null;
    }

    public BindPackage GetItemPkg(string name)
    {
       return new BindPackage { obj = GetItem(name) };
    }

    public T GetItem<T>(string name) where T : UnityEngine.Object
    {
        foreach (var item in bindList)
        {
            if (item.name == name)
            {
                T o = default;
                if (item.className.Equals("UnityEngine.GameObject")) o = item.obj as T;
                else o = item.obj.GetComponent(item.className) as T;
                return o;
            }
        }

        return null;
    }

    public void AutoBind<T>(T obj)
    {

        foreach (var item in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            object value = null;

            if (item.FieldType == typeof(Image)) value = GetItem(item.Name) as Image;
            if (item.FieldType == typeof(Button)) value = GetItem(item.Name) as Button;
            if (item.FieldType == typeof(GameObject)) value = GetItem(item.Name) as GameObject;
            if (item.FieldType == typeof(Text)) value = GetItem(item.Name) as Text;
            if (item.FieldType == typeof(RawImage)) value = GetItem(item.Name) as RawImage;
            if (item.FieldType == typeof(RectTransform)) value = GetItem(item.Name) as RectTransform;

            if (value == null) {
                Debug.LogError(item.FieldType.Name + "类型尚不支持自动绑定！");
                continue;
            }

            if(value!=null) item.SetValue(obj, value);
        }
    }
}
