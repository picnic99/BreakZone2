using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleTabItem : UIItemBase
{
    public GameObject root;
    public UIBinding bind;

    public RoleTabItem(GameObject obj)
    {
        root = obj;
        bind = root.GetComponent<UIBinding>();
        InitUI();
    }

    public void InitUI()
    {
        bind["head"].AS<Image>().color = Color.red;
    }

    public void UpdateData()
    {

    }

    public override void OnDestory()
    {
        bind = null;
        root = null;
        base.OnDestory();
    }
}
