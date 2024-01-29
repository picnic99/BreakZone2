using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseInfoSkillItem : UIItemBase
{
    public Image frame { get { return UIBase.GetBind<Image>(Root, "frame"); } }
    public Image Icon { get { return UIBase.GetBind<Image>(Root, "Icon"); } }
    public Image cdMask { get { return UIBase.GetBind<Image>(Root, "cdMask"); } }

    private SkillVO vo;
    private float baseCoolDown;

    public BaseInfoSkillItem(GameObject obj) : base(obj)
    {
        InitUI();
    }

    public override void InitUI()
    {

    }

    public void UpdateData(SkillVO vo)
    {
        this.vo = vo;
        Icon.sprite = ResourceManager.GetInstance().GetIcon(vo.GetSkillIcon());
        baseCoolDown = vo.baseCoolDown;
    }

    public void OnUpdateCD()
    {
        if (vo == null) return;

        var curCoolDown = SkillManager.GetInstance().GetSkillCoolDown(vo.skill.Id);
        if (curCoolDown > 0)
        {
            cdMask.fillAmount = curCoolDown / baseCoolDown;
        }
    }

}
