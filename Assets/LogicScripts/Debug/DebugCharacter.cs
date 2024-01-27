using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCharacter
{
    public GameObject Root;
    public Character character;

    public Text msg;
    public Button changeBtn;
    public Button selectBtn;
    public Button delBtn;
    public InputField msgInput;
    public Button applyBtn;

    public DebugCharacter(GameObject root)
    {
        Root = root;
        msg = root.transform.Find("msgView/Viewport/Content/msg").GetComponent<Text>();
        changeBtn = root.transform.Find("optView/changeBtn").GetComponent<Button>();
        selectBtn = root.transform.Find("optView/selectBtn").GetComponent<Button>();
        delBtn = root.transform.Find("optView/delBtn").GetComponent<Button>();
        msgInput = root.transform.Find("optView/msgInput").GetComponent<InputField>();
        applyBtn = root.transform.Find("optView/applyBtn").GetComponent<Button>();

        changeBtn.onClick.AddListener(Change);
        selectBtn.onClick.AddListener(Select);
        delBtn.onClick.AddListener(del);
    }

    public void UpdateData(Character character = null)
    {
        if (character == null && this.character == null) return;

        this.character = this.character == null ? character : this.character;

        this.character.OnUpdate();

        msg.text = this.character.msg.ShowMsg();

    }

    /// <summary>
    /// ÇÐ»»ÎªÖ÷½ÇÉ«
    /// </summary>
    public void Change()
    {
        if (character == null) return;
        if (GameContext.GameMode == GameMode.DEBUG)
        {
            DebugManager.GetInstance().SetMainRole(this);
        }
    }

    public void Select()
    {
        if (character == null) return;
        if (GameContext.GameMode == GameMode.DEBUG)
        {
            DebugManager.GetInstance().SelectTarget(character);
        }
    }

    public void del()
    {
        if (GameContext.GameMode == GameMode.DEBUG)
        {
            DebugManager.GetInstance().DelRole(this);
        }
        GameObject.Destroy(Root);
    }

    public void state()
    {

    }

    public void skill()
    {

    }

    public void buff()
    {

    }

    public void SetMainRole(bool b)
    {
        Root.GetComponent<Image>().color = b ? Color.yellow : Color.white;
    }
}

