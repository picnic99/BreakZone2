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
    private GameObject infoRoot { get { return UIBase.GetBind<GameObject>(Root, "infoRoot"); } }

    private GameObject tabItemObj;

    private RoleInfoView infoView;
    private UIBinding bind;

    private List<RoleTabItem> roleTabs;

    public SelectRoleUI()
    {
        uiPath = RegPrefabs.SelectRoleUI;
        layer = UILayers.BOTTOM;
        roleTabs = new List<RoleTabItem>();
    }

    public override void OnLoad()
    {
        base.OnLoad();
        bind = this.Root.GetComponent<UIBinding>();
        tabItemObj = bind["roleItem"].AS<GameObject>();

        var datas = CharacterConfiger.GetInstance().GetRealCharacters();

        for (int i = 0; i < datas.Length; i++)
        {
            var item = datas[i];
            var obj = GameObject.Instantiate<GameObject>(tabItemObj, tabItemObj.transform.parent);
            var roleItem = new RoleTabItem(obj);
            obj.SetActive(true);
            roleTabs.Add(roleItem);
            AddClick(roleItem.Item, OnTabClick, roleItem, item);
            roleItem.UpdateData(item);
            if (i == 0)
            {
                OnTabClick(roleItem, item);
            }
        }
    }

    public void OnTabClick(params object[] args)
    {
        RoleTabItem tab = args[0] as RoleTabItem;
        CharacterVO vo = args[1] as CharacterVO;

        foreach (var item in roleTabs)
        {
            item.IsSelect = item == tab;
            updateRightInfo(vo);
            updateRole(vo);
        }
        updateTabs();
    }

    private void updateTabs()
    {
        foreach (var item in roleTabs)
        {
            item.UpdateShow();
        }
    }

    private void updateRole(CharacterVO vo)
    {
        ShowCharacter crt = CharacterManager.GetInstance().AddShowRole(vo);
        crt.trans.parent = GameObject.Find("RolePos").transform;
    }

    private void updateRightInfo(CharacterVO vo)
    {
        if (infoView == null)
        {
            infoView = new RoleInfoView(infoRoot);
        }
        infoView.UpdateData(vo);
    }

    public override void OnUnLoad()
    {
        base.OnUnLoad();
    }
}
