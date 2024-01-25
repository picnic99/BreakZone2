using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>, Manager
{
    public static string SHOW_UI = "UIManager_SHOW_UI";
    public static string HIDE_UI = "UIManager_HIDE_UI";
    public static string CLOSE_UI = "UIManager_CLOSE_UI";

    public EventDispatcher eventer;

    public RootCanvas rootCvs;

    private Dictionary<string, UIBase> UIDic;

    public void Init()
    {
        UIDic = new Dictionary<string, UIBase>();
        eventer = new EventDispatcher();
        eventer.On(UIManager.SHOW_UI,ShowUI);
        eventer.On(UIManager.HIDE_UI, HideUI);
        eventer.On(UIManager.CLOSE_UI, CloseUI);

        //´´½¨RootCanvas
        rootCvs = new RootCanvas();
        rootCvs.OnLoad();
        rootCvs.Init();
    }

    public void ShowUI(object[] args)
    {
        string uiName = (string)args[0];
        Type type = ((RegUIClass)RegUIClass.GetInstance()).GetUIType(uiName);
        if (type != null)
        {
            UIBase ui = (UIBase)type.Assembly.CreateInstance(type.Name);
            var cvs = rootCvs.GetCvsByLayer(ui.layer);
            ui.Root.transform.SetParent(cvs.Root.transform);
            ui.Root.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            ui.Root.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
          UIDic.Add(uiName,ui);
            ui.OnLoad();
        }
    }

    public void ClearAllUI()
    {
        foreach (var item in UIDic)
        {
            var ui = item.Value;
            ui.OnUnLoad();
        }

        UIDic.Clear();
    }

    public void ShowUI(string uiName)
    {
        ShowUI(new object[] { uiName });
    }

    public void HideUI(object[] args)
    {
        string uiName = (string)args[0];
        if (UIDic.ContainsKey(uiName))
        {
            var ui = UIDic[uiName];
            ui.Hide();
        }
    }

    public void CloseUI(object[] args)
    {
        string uiName = (string)args[0];
        if (UIDic.ContainsKey(uiName))
        {
            var ui = UIDic[uiName];
            ui.OnUnLoad();
            UIDic.Remove(uiName);
        }
    }
}
