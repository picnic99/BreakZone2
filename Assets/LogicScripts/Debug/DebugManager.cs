using CustomPlayable;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : Manager<DebugManager>
{
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

    public DebugUI debugUI;

    private List<Func<string>> monitors = new List<Func<string>>();

    public void ShowPanel()
    {
        if (debugUI != null)
        {
            if (debugUI.Root.activeSelf)
            {
                debugUI.Hide();
            }
            else
            {
                debugUI.Show();
            }
            return;
        }

        debugUI = (DebugUI)UIManager.GetInstance().ShowUI(RegUIClass.DebugUI);
        var uiRoot = debugUI.Root;
        charaterView = uiRoot.transform.Find("bg/characterArea");
        recordView = uiRoot.transform.Find("bg/recordView");
        skillView = uiRoot.transform.Find("bg/skillPanel");
        globalOptView = uiRoot.transform.Find("bg/globalOptView");
        animView = uiRoot.transform.Find("bg/AnimView");
        monitorTips = uiRoot.transform.Find("bg/monitor/monitorTips").GetComponent<Text>();

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

        /*        DebugUIBinding.GetInstance().btn_skillEditor.onClick.AddListener(() => {
                    DebugUIBinding.GetInstance().obj_opt.SetActive(false);
                    DebugUIBinding.GetInstance().obj_skillEditor.SetActive(true);
                    DebugUIBinding.GetInstance().obj_skillEditorStage.SetActive(true);
                });

                DebugUIBinding.GetInstance().btn_backScene.onClick.AddListener(() => {

                    DebugUIBinding.GetInstance().obj_opt.SetActive(true);
                    DebugUIBinding.GetInstance().obj_skillEditor.SetActive(false);
                    DebugUIBinding.GetInstance().obj_skillEditorStage.SetActive(false);
                });

                DebugUIBinding.GetInstance().animDebugBtn.onClick.AddListener(() => {
                    animView.gameObject.SetActive(!animView.gameObject.activeInHierarchy);
                });
        */
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
        if (vo.id == 99 && GameContext.CurRole != null)
        {
            character.physic.Move(GameContext.CurRole.trans.position + GameContext.CurRole.trans.forward * 3f,0.1f);
        }
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
            GameContext.CurRole = mainRole.character;
            CameraManager.GetInstance().ShowMainCam();
/*            foreach (var c in GameContext.AllCharacter)
            {
                if (c == GameContext.SelfRole) c.state = CharacterState.FRIEND;
                else c.state = CharacterState.ENEMY;
            }*/
            //EventDispatcher.GetInstance().Event(EventDispatcher.MAIN_ROLE_CHANGE);
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
