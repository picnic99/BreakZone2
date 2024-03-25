using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTargetItem
{
    public _Character character;
    public GameObject Root;
    public Button removeBtn;
    public Text nameTxt;

    public DebugTargetItem(GameObject Root,_Character character)
    {
        this.Root = Root;
        this.character = character;

        removeBtn = this.Root.GetComponent<Button>();
        nameTxt = this.Root.transform.Find("targetName").GetComponent<Text>();
        nameTxt.text = this.character.characterData.characterName;
        removeBtn.onClick.AddListener(Remove);
    }

    public void Remove()
    {
        if (this.character == null) return;
        if(GameContext.GameMode == GameMode.DEBUG)
        {
            DebugManager.GetInstance().RemoveTarget(this);
        }
        GameObject.Destroy(this.Root);
    }
}
