using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoleInfoView : UIItemBase
{
    public GameObject enterBtn { get { return UIBase.GetBind<GameObject>(Root, "enterBtn"); } }
    public Image baseInfoHead { get { return UIBase.GetBind<Image>(Root, "baseInfoHead"); } }
    public TextMeshProUGUI baseInfoName { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "baseInfoName"); } }
    public TextMeshProUGUI baseInfoDesc { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "baseInfoDesc"); } }
    public GameObject skillItem { get { return UIBase.GetBind<GameObject>(Root, "skillItem"); } }
    public GameObject skillViewContent { get { return UIBase.GetBind<GameObject>(Root, "skillContent"); } }

    private CharacterVO vo;

    private List<RoleSkillItem> skillItems;

    public RoleInfoView(GameObject obj) : base(obj)
    {
        InitUI();
    }

    public override void InitUI()
    {
        UIBase.AddClick(enterBtn, EnterRoom);
        skillItems = new List<RoleSkillItem>();
    }

    public void EnterRoom(object[] args)
    {
        SceneManager.GetInstance().ChangeScene("GameRoom",()=>
        {
            var go = new GameObject();
            Character crt = CharacterManager.GetInstance().CreateCharacter(vo);
            crt.trans.SetParent(go.transform);
            crt.trans.localPosition = Vector3.zero;

        });
    }

    public void UpdateData(CharacterVO vo)
    {
        this.vo = vo;
        baseInfoName.text = vo.character.Name;
        baseInfoDesc.text = vo.character.Desc;

        UpdateSkillInfo();
    }

    public void UpdateSkillInfo()
    {
        if (vo == null) return;
        var skills = vo.GetSkillArr();
        ClearSkillItems();
        foreach (var item in skills)
        {
            var skillVO = SkillConfiger.GetInstance().GetSkillById(item);
            if(skillVO != null)
            {
                if (skillVO.skill.IsShow)
                {
                    var obj = GameObject.Instantiate<GameObject>(skillItem, skillItem.transform.parent);
                    var skill = new RoleSkillItem(obj);
                    obj.SetActive(true);
                    skillItems.Add(skill);
                    skill.UpdateData(skillVO);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(skillViewContent.GetComponent<RectTransform>());
        }
    }

    public void ClearSkillItems()
    {
        foreach (var item in skillItems)
        {
            MonoBridge.GetInstance().DestroyOBJ(item.Root);
            item.OnDestory();
        }
        skillItems.Clear();
    }

}
