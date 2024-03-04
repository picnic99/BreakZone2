using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Client.Net.PB.Enum;
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
    private GameObject infoRoot => UIBase.GetBind<GameObject>(Root, "infoRoot");
    private GameObject LoginOutBtn => UIBase.GetBind<GameObject>(Root, "LoginOutBtn");

    private GameObject tabItemObj;

    private RoleInfoView infoView;
    private UIBinding bind;

    private List<RoleTabItem> roleTabs;

    private Character curCrt;

    public SelectRoleUI()
    {
        uiPath = RegPrefabs.SelectRoleUI;
        belongScene = RegSceneClass.SelectRoleScene;
        layer = UILayers.BOTTOM;
        roleTabs = new List<RoleTabItem>();
    }

    public override void OnLoad()
    {
        base.OnLoad();
        bind = this.Root.GetComponent<UIBinding>();
        tabItemObj = bind["roleItem"].AS<GameObject>();

        var datas = CharacterConfiger.GetInstance().GetRealCharacters();
        AddClick(LoginOutBtn, OnLoginOut);

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
            if (item.IsSelect)
            {
                updateRightInfo(vo);
                updateRole(vo);
            }
        }
        updateTabs();
    }
    public void OnLoginOut(object[] args)
    {
        Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().SendLoginReq(PlayerManager.GetInstance().Self.username, "", LoginTypeEnum.LOGIN_Out);
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
        curCrt?.OnDestory();
        Character crt = CharacterManager.GetInstance().CreateCharacter(vo);
        curCrt = crt;
        curCrt.trans.SetParent(GameObject.Find("RolePos").transform);
        curCrt.trans.localPosition = Vector3.zero;

        //GameContext.CurRole = crt;
        GameContext.SelectCrtId = vo.id;
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
        CharacterManager.GetInstance().RemoveCharacter(curCrt);
        base.OnUnLoad();
    }
}
