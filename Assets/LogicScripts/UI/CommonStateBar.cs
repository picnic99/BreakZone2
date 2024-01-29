using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonStateBar:UIBase
{
    private RectTransform HPprogress { get { return UIBase.GetBind<GameObject>(Root, "progress").GetComponent<RectTransform>(); } }
    private TextMeshProUGUI simpleStateTxt { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "simpleStateTxt"); } }
    private TextMeshProUGUI fullMsg { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "fullTxt"); } }

    private Character character;

    public CommonStateBar(Character character)
    {
        uiPath = "stateBar/CommonStateBar";
        this.character = character;
        InitUI();
    }

    private void InitUI()
    {
        if (character == null)
        {
            Debug.LogError(this.GetType().Name + "£∫CharacterŒ¥…Ë÷√£°");
            return;
        }
        if(Root == null)
        {
            Debug.LogError(this.GetType().Name + "£∫UIRoot == NULL£°");
            return;
        }
        this.Root.transform.parent = this.character.trans;
        this.Root.transform.position = Vector3.zero + Vector3.up * 2f;
    }

    public override void OnUpdate()
    {
        //≥ØœÚ…„œÒª˙
        Root.transform.forward = Root.transform.position - CameraManager.GetInstance().mainCam.transform.position;
        fullMsg.text = this.character.msg.ShowSimpleMsg();
        simpleStateTxt.text = "";
        HPprogress.sizeDelta = new Vector2(this.character.property.hp.finalValue / this.character.property.hp.baseValue * 100, HPprogress.sizeDelta.y);
    }
}