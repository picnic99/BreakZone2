using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleSkillItem : UIItemBase
{
    public GameObject Head { get { return UIBase.GetBind<GameObject>(Root, "head"); } }
    public TextMeshProUGUI Name { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "name"); } }
    public TextMeshProUGUI Desc { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "desc"); } }

    public RoleSkillItem(GameObject obj) : base(obj)
    {
        InitUI();
    }

    public override void InitUI()
    {

    }

    public void UpdateData(SkillVO vo)
    {
        Name.text = vo.skill.Name;
        Desc.text = vo.skill.Desc;
    }
}
