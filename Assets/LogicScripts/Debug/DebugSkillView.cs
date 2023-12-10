using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSkillView
{
    public Character character;
    public GameObject tmp_skillItem;
    private List<DebugSkillItem> skillItems;

    public DebugSkillView(GameObject tmp_skillItem)
    {
        this.tmp_skillItem = tmp_skillItem;
        skillItems = new List<DebugSkillItem>();
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        UpdateSkillItems();
    }

    private void UpdateSkillItems()
    {
        if (this.character == null)
        {
            foreach (var item in skillItems)
            {
                item.Root.SetActive(false);
            }
            return;
        }
        var skill = character.characterData.GetSkillArr();
        for (int i = 0; i < skill.Count; i++)
        {
            DebugSkillItem item = null;
            if (skillItems.Count - 1 < i)
            {
                GameObject obj = GameObject.Instantiate<GameObject>(tmp_skillItem, tmp_skillItem.transform.parent);
                obj.SetActive(true);
                item = new DebugSkillItem(obj, i == 0 ? StateType.DoAtk : StateType.DoSkill);
                skillItems.Add(item);
            }
            else
            {
                item = skillItems[i];
            }
            item.Root.SetActive(true);
            item.UpdateItem(skill[i], i);
        }
    }

}
