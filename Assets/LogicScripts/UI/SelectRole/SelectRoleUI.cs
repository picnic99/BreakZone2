using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择角色UI
/// 1、左侧区域点击会切换角色 同时更改右侧信息
/// 2、点击进入大厅即可进入到大厅中
/// </summary>
public class SelectRoleUI : UIBase
{
    private RoleTabItem tabItem;
    private UIBinding bind;

    public SelectRoleUI()
    {
        uiPath = RegPrefabs.SelectRoleUI;
        layer = UILayers.BOTTOM;
    }

    public override void OnLoad()
    {
        base.OnLoad();
        bind = this.UIRoot.GetComponent<UIBinding>();
        new RoleTabItem(bind["roleItem"].AS<GameObject>());
    }

    public override void OnUnLoad()
    {
        base.OnUnLoad();
    }
}
