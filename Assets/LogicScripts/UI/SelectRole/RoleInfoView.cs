using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleInfoView : UIItemBase
{
    public GameObject enterBtn { get { return UIBase.GetBind<GameObject>(Root, "enterBtn"); } }
    public Image baseInfoHead { get { return UIBase.GetBind<Image>(Root, "baseInfoHead"); } }
    public TextMeshProUGUI baseInfoName { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "baseInfoName"); } }
    public TextMeshProUGUI baseInfoDesc { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "baseInfoDesc"); } }
    public GameObject skillItem { get { return UIBase.GetBind<GameObject>(Root, "skillItem"); } }
    public RectTransform skillViewContent { get { return UIBase.GetBind<RectTransform>(Root, "skillViewContent"); } }

    public RoleInfoView(GameObject obj) : base(obj)
    {
        InitUI();
    }

    public override void InitUI()
    {
        UIBase.AddClick(enterBtn, (object[] args) => { Debug.Log(1); });
    }

    public void UpdateData(CharacterVO vo)
    {
        baseInfoName.text = vo.character.Name;
        baseInfoDesc.text = vo.character.Desc;
    }

}
