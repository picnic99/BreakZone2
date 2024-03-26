using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理器
/// </summary>
public class UIManager : Manager<UIManager>
{
    public static string SHOW_UI = "UIManager_SHOW_UI";
    public static string HIDE_UI = "UIManager_HIDE_UI";
    public static string CLOSE_UI = "UIManager_CLOSE_UI";
    public static string ADD_POP_VALUE = "UIManager_ADD_POP_VALUE";

    public RootCanvas rootCvs;

    private Dictionary<string, UIBase> UIDic;

    public override void Init()
    {
        UIDic = new Dictionary<string, UIBase>();
        /*       eventer.On(UIManager.SHOW_UI,ShowUI);
                eventer.On(UIManager.HIDE_UI, HideUI);
                eventer.On(UIManager.CLOSE_UI, CloseUI);*/

        //初始化RootCanvas
        rootCvs = new RootCanvas();
        rootCvs.OnLoad();
        rootCvs.Init();

        base.Init();
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        CharacterManager.Eventer.On(CharacterEvent.PROPERTY_CHANGE, OnCrtValueChange);
        GameSceneManager.Eventer.On(GameSceneManager.UNLOAD_SCENE, RemoveUIByScene);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        CharacterManager.Eventer.Off(CharacterEvent.PROPERTY_CHANGE, OnCrtValueChange);
        GameSceneManager.Eventer.Off(GameSceneManager.UNLOAD_SCENE, RemoveUIByScene);
    }

    private void OnCrtValueChange(object[] args)
    {
        Eventer.Event(ADD_POP_VALUE, args);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (var item in UIDic)
        {
            item.Value.OnUpdate();
        }
    }

    public void OnLaterUpdate()
    {
         
    }

    public UIBase ShowUI(object[] args)
    {
        string uiName = (string)args[0];
        string sceneName = args.Length >= 2 ? (string)args[1] : null;
        Type type = ((RegUIClass)RegUIClass.GetInstance()).Get(uiName);
        if (type != null)
        {
            UIBase ui = (UIBase)type.Assembly.CreateInstance(type.Name);
            var cvs = rootCvs.GetCvsByLayer(ui.layer);
            ui.Root.transform.SetParent(cvs.Root.transform);
            ui.Root.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            ui.Root.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            //AddToDic(uiName, ui, sceneName);
            if (sceneName != null) ui.belongScene = sceneName;
            UIDic.Add(uiName, ui);
            ui.OnLoad();
            GameContext.CurScene?.SceneUIs.Add(uiName);
            return ui;
        }

        return null;
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

    public UIBase ShowUI(string uiName)
    {
        return ShowUI(new object[] { uiName });
    }
    public UIBase ShowUI(string uiName, string sceneName)
    {
        return ShowUI(new object[] { uiName, sceneName });
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
        CloseUI(uiName);
    }

    public void CloseUI(string uiName)
    {
        if (UIDic.ContainsKey(uiName))
        {
            var ui = UIDic[uiName];
            ui.OnUnLoad();
            UIDic.Remove(uiName);
        }
    }

    /// <summary>
    /// 移除场景相关的所有UI
    /// </summary>
    /// <param name="args"></param>
    public void RemoveUIByScene(object[] args)
    {
        string sceneUI = args[0] as string;
        List<string> result = new List<string>();
        foreach (var item in UIDic)
        {
            if(item.Value.belongScene == sceneUI)
            {
                result.Add(item.Key);
                item.Value.OnUnLoad();
            }
        }

        foreach (var item in result)
        {
            UIDic.Remove(item);
        }
    }



    /// <summary>
    /// UI是否显示
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public bool IsUIShow(string uiName)
    {
        if (UIDic.ContainsKey(uiName))
        {
            var ui = UIDic[uiName];
            return ui.Root.activeSelf;
        }
        return false;
    }
}
