using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSkillItem
{
    public GameObject Root;
    public Button skillBtn;
    public Text skillName;
    public Image cdMask;

    public int skillId = 0;
    public int skillIndex = -1;

    public string stateName;
    private float baseCoolDown;

    public DebugSkillItem(GameObject root, string stateName)
    {
        if (root == null) return;
        Root = root;
        this.stateName = stateName;
        skillBtn = root.transform.GetComponent<Button>();
        skillName = root.transform.Find("skillName").GetComponent<Text>();
        cdMask = root.transform.Find("cdMask").GetComponent<Image>();
        skillBtn.onClick.AddListener(OnClick);
        UpdateCoolDown();
    }

    public void UpdateItem(int id = -1, int skillIndex = -1)
    {
        if (id == -1 && skillId <= 0)
        {
            return;
        }

        skillId = id;
        this.skillIndex = skillIndex;

        var vo = SkillConfiger.GetInstance().GetSkillById(skillId);
        baseCoolDown = vo.baseCoolDown;
        skillName.text = vo.SkillName;
        cdMask.fillAmount = 0;

    }

    private void UpdateCoolDown()
    {
        TimeManager.GetInstance().AddLoopTimer(this, 0, () =>
        {
            if (this.skillIndex == -1) return;
            var curCoolDown = SkillManager.GetInstance().GetSkillCoolDown(skillId);
            if (curCoolDown > 0)
            {
                cdMask.fillAmount = curCoolDown / baseCoolDown;
            }
        });
    }

    private void OnClick()
    {
        if (this.skillIndex >= 0)
        {
            InputManager.GetInstance().ChangeState(GameContext.SelfRole, stateName, this.skillIndex);
        }
        else
        {
            InputManager.GetInstance().ChangeState(GameContext.SelfRole, stateName);
        }
    }
}
