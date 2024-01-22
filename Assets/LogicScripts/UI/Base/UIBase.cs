using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase
{
    public string uiPath;
    public UILayers layer;
    private GameObject root;
    public GameObject UIRoot
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

    public void AddClick(GameObject obj, Action<PointerEventData> call)
    {
        if (obj != null)
        {
            if (!obj.TryGetComponent<UIInteract>(out UIInteract interact))
            {
                interact = obj.AddComponent<UIInteract>();
            }
            interact.moveCall = call;
        }
    }
    public void RemoveClick(GameObject obj, Action<PointerEventData> call)
    {
        if (obj != null)
        {
            if (obj.TryGetComponent<UIInteract>(out UIInteract interact))
            {
                GameObject.Destroy(interact);
            }
        }
    }

    public void AddDrag()
    {

    }
}