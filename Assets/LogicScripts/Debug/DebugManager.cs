using CustomPlayable;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    public Cinemachine.CinemachineFreeLook cameraCtrl;

    private Transform charaterView;
    private Transform recordView;
    private Transform skillView;
    private Transform globalOptView;
    private Transform skillEditorView;
    private Transform animView;

    private Transform tmp_character;
    private Transform tmp_record;
    private Transform tmp_skill;
    private Transform tmp_target;
    private Transform tmp_anim;
    private Text monitorTips;

    private DebugSkillView debugSkillView;
    private DebugRoleView debugRoleView;
    private DebugTargetView debugTargetView;
    private DebugRecordView debugRecordView;
    private DebugAnimView debugAnimView;

    public DebugCharacter mainRole;

    private List<Func<string>> monitors = new List<Func<string>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        charaterView = transform.Find("bg/characterArea");
        recordView = transform.Find("bg/recordView");
        skillView = transform.Find("bg/skillPanel");
        globalOptView = transform.Find("bg/globalOptView");
        animView = transform.Find("bg/AnimView");
        monitorTips = transform.Find("bg/monitor/monitorTips").GetComponent<Text>();

        tmp_character = charaterView.Find("character");
        tmp_record = recordView.Find("Viewport/Content/record");
        tmp_skill = skillView.Find("skill");
        tmp_target = globalOptView.Find("targetArea/Scroll View/Viewport/Content/target");
        tmp_anim = animView.Find("Viewport/Content/animNode");

        tmp_character.gameObject.SetActive(false);
        tmp_record.gameObject.SetActive(false);
        tmp_skill.gameObject.SetActive(false);
        tmp_target.gameObject.SetActive(false);

        debugSkillView = new DebugSkillView(tmp_skill.gameObject);
        debugRoleView = new DebugRoleView(tmp_character.gameObject);
        debugTargetView = new DebugTargetView(tmp_target.gameObject);
        debugRecordView = new DebugRecordView(tmp_record.gameObject, recordView.gameObject);
        debugAnimView = new DebugAnimView(tmp_anim.gameObject);

        UIBinding.GetInstance().btn_skillEditor.onClick.AddListener(() => {
            UIBinding.GetInstance().obj_opt.SetActive(false);
            UIBinding.GetInstance().obj_skillEditor.SetActive(true);
            UIBinding.GetInstance().obj_skillEditorStage.SetActive(true);
        });

        UIBinding.GetInstance().btn_backScene.onClick.AddListener(() => {

            UIBinding.GetInstance().obj_opt.SetActive(true);
            UIBinding.GetInstance().obj_skillEditor.SetActive(false);
            UIBinding.GetInstance().obj_skillEditorStage.SetActive(false);
        });

        UIBinding.GetInstance().animDebugBtn.onClick.AddListener(() => {
            animView.gameObject.SetActive(!animView.gameObject.activeInHierarchy);
        });

    }

    public void AddMonitor(Func<string> valueGet)
    {
        monitors.Add(valueGet);
    }

    public void AddAnimMonitor(AnimMixNode node)
    {
        debugAnimView.AddAnimMonitor(node);
    }

    /// <summary>
    /// 添加一个角色
    /// </summary>
    public Character AddRoleReturn()
    {
        var idInput = globalOptView.Find("characterIdInput").GetComponent<InputField>().text;

        //idInput = "1";
        var vo = CharacterConfiger.GetInstance().GetCharacterById(Convert.ToInt32(idInput));
        var obj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.modelName);
        //if (vo.id == 99)obj = tmp_fakeMan_obj;
        var c_obj = obj;
        var character = new Character(vo, c_obj);
        debugRoleView.AddRole(character);
        GameContext.AllCharacter.Add(character);
        EventDispatcher.GetInstance().Event(EventDispatcher.MAIN_ROLE_CHANGE);
        return character;
    }

    public void AddRole()
    {
        var idInput = globalOptView.Find("characterIdInput").GetComponent<InputField>().text;

        //idInput = "1";
        var vo = CharacterConfiger.GetInstance().GetCharacterById(Convert.ToInt32(idInput));
        var obj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.modelName);
        //if (vo.id == 99) obj = tmp_fakeMan_obj;
        var c_obj = obj;
        var character = new Character(vo, c_obj);
/*        character.trans.gameObject.AddComponent<FullBodyBipedIK>();
        character.trans.gameObject.AddComponent<GrounderFBBIK>();*/
        debugRoleView.AddRole(character);
        GameContext.AllCharacter.Add(character);
        EventDispatcher.GetInstance().Event(EventDispatcher.MAIN_ROLE_CHANGE);
    }

    private void Update()
    {
        InputManager.GetInstance().OnUpdate();
        debugRoleView.UpdateData();
        debugAnimView.OnUpdate();
        //ActionManager.GetInstance().SettleAction();
        monitorTips.text = "";
        foreach (var item in monitors)
        {
            monitorTips.text += item() + "\n";
        }
    }

    private void updateCharacter()
    {

    }

    public void ChangeCurRole()
    {

    }

    public void SetMainRole(DebugCharacter character)
    {
        if (mainRole != null) mainRole.SetMainRole(false);
        mainRole = character;
        debugSkillView.SetCharacter(character != null ? character.character : null);
        if (mainRole != null)
        {
            mainRole.SetMainRole(true);
            GameContext.SelfRole = mainRole.character;
            CameraManager.GetInstance().ShowMainCam(GameContext.SelfRole);
/*            foreach (var c in GameContext.AllCharacter)
            {
                if (c == GameContext.SelfRole) c.state = CharacterState.FRIEND;
                else c.state = CharacterState.ENEMY;
            }*/
            EventDispatcher.GetInstance().Event(EventDispatcher.MAIN_ROLE_CHANGE);
        }
    }

    public void DoSkill(int index)
    {
        if (mainRole == null) return;
        mainRole.character.eventDispatcher.Event(CharacterEvent.DO_SKILL, index);
    }

    public void RemoveTarget(DebugTargetItem item)
    {
        debugTargetView.RemoveTarget(item);
    }

    public void SelectTarget(Character character)
    {
        debugTargetView.AddTarget(character);
    }

    public Character[] GetTargets()
    {
        return debugTargetView.GetTargets().ToArray();
    }

    public void DelRole(DebugCharacter character)
    {
        var targetItem = debugTargetView.GetTarget(character.character);
        if (targetItem != null) targetItem.Remove();
        debugRoleView.DelRole(character);
        if (character == mainRole)
        {
            //debugSkillView.SetCharacter(debugRoleView.GetFristRole() != null ? debugRoleView.GetFristRole().character : null);
            SetMainRole(debugRoleView.GetFristRole() != null ? debugRoleView.GetFristRole() : null);
        }
    }
}
