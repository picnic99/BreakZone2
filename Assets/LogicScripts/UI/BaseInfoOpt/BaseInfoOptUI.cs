using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseInfoOptUI : UIBase
{
    private TextMeshProUGUI crtName { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "crtName"); } }
    private Image head { get { return UIBase.GetBind<Image>(Root, "head"); } }
    private RectTransform hpBar { get { return UIBase.GetBind<GameObject>(Root, "hpBar").GetComponent<RectTransform>(); } }
    private GameObject skillItem { get { return UIBase.GetBind<GameObject>(Root, "skillItem"); } }
    private GameObject buffItem { get { return UIBase.GetBind<GameObject>(Root, "buffItem"); } }
    private GameObject exitBtn { get { return UIBase.GetBind<GameObject>(Root, "exitBtn"); } }
    private GameObject PopValuePanel { get { return UIBase.GetBind<GameObject>(Root, "PopValuePanel"); } }

    CharacterVO vo;

    private List<BaseInfoSkillItem> skillItems;

    public BaseInfoOptUI()
    {
        uiPath = RegPrefabs.BaseInfoOptUI;
        layer = UILayers.MIDDLE;

        skillItems = new List<BaseInfoSkillItem>();

        InitUI();

        UIManager.Eventer.On(UIManager.ADD_POP_VALUE, OnAddPopValue);
    }

    public override void OnUnLoad()
    {
        UIManager.Eventer.Off(UIManager.ADD_POP_VALUE, OnAddPopValue);
        base.OnUnLoad();
    }


    public void InitUI()
    {
        vo = GameContext.CurRole.characterData;
        //更新头像与昵称
        crtName.text = vo.character.Name;
        //技能信息
        UpdateSkillInfo();
        //buff信息处理

        //操作按钮部分处理
        AddClick(exitBtn, OnExitBtnClick);
        //小地图开发

        //返回选择页面

        //DEBUG 新增为指定角色添加指定buff

        //DEBUG 新增为指定角色修改属性值

    }

    private void OnAddPopValue(object[] args)
    {
        PropertyType type = (PropertyType)args[0];
        PropertyValue value = args[1] as PropertyValue;
        Character crt = args[2] as Character;

        new PopValue(PopValuePanel,crt.trans,value.finalValue);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (var item in skillItems)
        {
            item.OnUpdateCD();
        }
    }

    public void UpdateSkillInfo()
    {
        if (vo == null) return;
        var skills = vo.GetSkillArr();
        ClearSkillItems();
        foreach (var item in skills)
        {
            var skillVO = SkillConfiger.GetInstance().GetSkillById(item);
            if (skillVO != null)
            {
                if (skillVO.skill.IsShow)
                {
                    var obj = GameObject.Instantiate<GameObject>(skillItem, skillItem.transform.parent);
                    var skill = new BaseInfoSkillItem(obj);
                    obj.SetActive(true);
                    skillItems.Add(skill);
                    skill.UpdateData(skillVO);
                }
            }
            //LayoutRebuilder.ForceRebuildLayoutImmediate(skillViewContent.GetComponent<RectTransform>());
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

    public void OnExitBtnClick(object[] args)
    {
        GameSceneManager.GetInstance().SwitchScene(RegSceneClass.SelectRoleScene);
    }
}
