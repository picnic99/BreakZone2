using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase
{
    public string uiPath;
    public UILayers layer;
    private GameObject root;
    public GameObject Root
    {
        get
        {
            if (root != null) return root;
            if (uiPath != null && uiPath != "")
            {
                GameObject obj = ResourceManager.GetInstance().GetObjInstance<GameObject>("UI/" + uiPath);
                root = obj;
                return root;
            }
            return null;
        }
    }

    public UIBase()
    {

    }

    public UIBase(GameObject obj)
    {
        this.root = obj;
    }

    public virtual void OnLoad()
    {
        Show();
    }

    public virtual void OnUnLoad()
    {
        if (this.root != null)
        {
            MonoBridge.GetInstance().DestroyOBJ(this.root);
        }
    }

    public virtual void Show()
    {
        if (this.root != null)
        {
            this.root.SetActive(true);
        }
    }

    public virtual void Hide()
    {
        if (this.root != null)
        {
            this.root.SetActive(false);
        }
    }

    public static void AddClick(GameObject obj, Action<object[]> call,params object[] args)
    {
        if (obj != null)
        {
            if (!obj.TryGetComponent<UIInteract>(out UIInteract interact))
            {
                interact = obj.AddComponent<UIInteract>();
            }

            Action<PointerEventData> temp = (x) => call(args);

            interact.clickCall = temp;
        }
    }
    public static void RemoveClick(GameObject obj, Action<PointerEventData> call)
    {
        if (obj != null)
        {
            if (obj.TryGetComponent<UIInteract>(out UIInteract interact))
            {
                GameObject.Destroy(interact);
            }
        }
    }

    public static void AddMouseEnter(GameObject obj, Action<object[]> call, params object[] args)
    {
        if (obj != null)
        {
            if (!obj.TryGetComponent<UIInteract>(out UIInteract interact))
            {
                interact = obj.AddComponent<UIInteract>();
            }

            Action<PointerEventData> temp = (x) => call(args);

            interact.enterCall = temp;
        }
    }

    public static T GetBind<T>(GameObject obj, string itemName) where T : UnityEngine.Object
    {
        if (obj.TryGetComponent<UIBinding>(out UIBinding bind))
        {
            var result = bind[itemName].AS<T>();
            if (result == null)
            {
                Debug.LogError(itemName + " bind获取失败 请确认绑定是否正确！");
                return null;
            }
            return result;
        }
        return null;
    }

    public static void AddMouseExit(GameObject obj, Action<object[]> call, params object[] args)
    {
        if (obj != null)
        {
            if (!obj.TryGetComponent<UIInteract>(out UIInteract interact))
            {
                interact = obj.AddComponent<UIInteract>();
            }

            Action<PointerEventData> temp = (x) => call(args);

            interact.exitCall = temp;
        }
    }

    public void AddDrag()
    {

    }
}