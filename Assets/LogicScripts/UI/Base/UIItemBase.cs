using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemBase
{
    public GameObject Root;
    public UIBinding Bind;

    private RectTransform rootRect;
    public RectTransform RootRect
    {
        get
        {
            if (rootRect == null)
            {
                rootRect = Root.GetComponent<RectTransform>();
            }
            return rootRect;
        }
    }

    public UIItemBase()
    {
    }
    public UIItemBase(GameObject obj)
    {
        Root = obj;
        Bind = Root.GetComponent<UIBinding>();
    }

    public virtual void InitUI()
    {

    }
    public virtual void OnDestory()
    {
        Root = null;
        Bind = null;
    }
}
