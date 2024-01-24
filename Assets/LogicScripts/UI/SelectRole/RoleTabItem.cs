using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleTabItem : UIItemBase
{
    public GameObject Item { get { return UIBase.GetBind<GameObject>(Root, "item"); } }
    public TextMeshProUGUI Name { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "name"); } }

    public bool IsSelect = false;

    public RoleTabItem(GameObject obj) : base(obj)
    {
        InitUI();
    }

    public override void InitUI()
    {
        UIBase.AddMouseEnter(Item, (object[] args) => OnEnter());
        UIBase.AddMouseExit(Item, (object[] args) => OnExit());
    }

    public void UpdateData(CharacterVO vo)
    {
        Name.text = vo.character.Name;
    }

    public void UpdateShow()
    {
        Item.GetComponent<Image>().color = IsSelect? new Color(1, 0.83f, 0.4f): new Color(0.25f, 0.445f, 0.594f);
        Item.GetComponent<RectTransform>().anchoredPosition = IsSelect? new Vector2(50, 0): new Vector2(0, 0);
    }

    public void OnEnter()
    {
        if (!IsSelect)
        {
            Item.GetComponent<Image>().color = new Color(0.4f, 0.6f, 0.73f);
            Item.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, 0);
        }
    }
    public void OnExit()
    {
        if (!IsSelect)
        {
            Item.GetComponent<Image>().color = new Color(0.25f, 0.445f, 0.594f);
            Item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
    }
}
