using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ѡ���ɫUI
/// 1��������������л���ɫ ͬʱ�����Ҳ���Ϣ
/// 2���������������ɽ��뵽������
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
