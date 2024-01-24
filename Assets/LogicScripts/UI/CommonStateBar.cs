using UnityEngine;
using UnityEngine.UI;

public class CommonStateBar:UIBase
{
    private Image HPprogress;
    private TMPro.TextMeshProUGUI simpleStateTxt;
    private TMPro.TextMeshProUGUI fullMsg;

    private Character character;

    public CommonStateBar(Character character)
    {
        uiPath = "stateBar/CommonStateBar";
        this.character = character;
        InitUI();
    }

    private void InitUI()
    {
        this.Root.transform.parent = this.character.trans;
        this.Root.transform.position = Vector3.zero + Vector3.up * 2f;
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

        simpleStateTxt = Root.transform.Find("simpleStateTxt").GetComponent<TMPro.TextMeshProUGUI>();
        fullMsg = Root.transform.Find("fullState/fullTxt").GetComponent<TMPro.TextMeshProUGUI>();
        HPprogress = Root.transform.Find("HPBar/progress").GetComponent<Image>();
    }

    public void OnUpdate()
    {
        //≥ØœÚ…„œÒª˙
        Root.transform.forward = Root.transform.position - CameraManager.GetInstance().mainCam.transform.position;
        fullMsg.text = this.character.msg.ShowSimpleMsg();
        simpleStateTxt.text = "";
        HPprogress.fillAmount = this.character.property.hp.finalValue / this.character.property.hp.baseValue;
    }
}