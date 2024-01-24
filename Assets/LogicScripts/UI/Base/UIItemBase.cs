using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemBase
{
    public GameObject Root;
    public UIBinding Bind;

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
