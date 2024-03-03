using Assets.LogicScripts.Client.Net.PB.Enum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 选择角色UI
/// 1、左侧区域点击会切换角色 同时更改右侧信息
/// 2、点击进入大厅即可进入到大厅中
/// </summary>
public class LoginUI : UIBase
{
    //private GameObject infoRoot { get { return UIBase.GetBind<GameObject>(Root, "infoRoot"); } }
    private TextMeshProUGUI Username => UIBase.GetBind<TextMeshProUGUI>(Root, "Username");
    private TextMeshProUGUI Password => UIBase.GetBind<TextMeshProUGUI>(Root, "Password");
    private GameObject LoginBtn => UIBase.GetBind<GameObject>(Root, "LoginBtn");

    public LoginUI()
    {
        uiPath = RegPrefabs.LoginUI;
        belongScene = RegSceneClass.LoginScene;
        layer = UILayers.BOTTOM;
    }

    public override void OnLoad()
    {
        base.OnLoad();
        AddClick(LoginBtn, OnLogin);
    }

    private void OnLogin(object[] args)
    {
        Debug.Log(Username.text);
        Debug.Log(Password.text);
        Assets.LogicScripts.Client.ActionManager.GetInstance().SendLoginReq(Username.text, Password.text,LoginTypeEnum.LOGIN_IN);
    }
}