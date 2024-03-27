using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    FRIEND, //����
    ENEMY, //����
    NEUTRAL //����
}

public class CharacterBaseInfo
{
    public bool isDebug = false;
    public bool canControl = false;
    public bool needControl = false;
    public bool isNeedStateBar = false;

    public static CharacterBaseInfo GetShowBaseInfo()
    {
        CharacterBaseInfo info = new CharacterBaseInfo();
        return info;
    }

    public static CharacterBaseInfo GetFightBaseInfo()
    {
        CharacterBaseInfo info = new CharacterBaseInfo();
        info.canControl = true;
        info.needControl = true;
        info.isNeedStateBar = true;
        return info;
    }
}

/// <summary>
/// ��ɫ������
/// 
/// </summary>
public class _Character
{
    public CharacterBaseInfo baseInfo { get; set; }

    public CharacterState state = CharacterState.NEUTRAL;
    //���ڿ��ƽ�ɫλ��
    public Transform trans { get; set; }
    //���ڲ��Ž�ɫ����
    public Animator anim { get; set; }
    //��¼��ɫ������ֵ
    public Property property { get; set; }
    //��Χ�����
    //public RangeScan scan;
    //���������
    public PhysicController physic { get; set; }

    public CharacterAnimator characterAnimator { get; set; }
    //�¼�������
    public _EventDispatcher eventDispatcher;
    //״̬�� ֻ�����ɫ״̬֮����л�
    public FSM fsm { get; set; }
    //StateBar
    //public CommonStateBar stateBar { get; set; }
    //���������
    public InputManager input { get; set; }

    //�洢��ɫ��ǰ����ִ�е�Skill ��Щ���ּ��� ��һ������ô�������
    public List<Skill> SkillBehaviour;
    //�洢��ɫ��ǰ����ִ�е�BUFF
    public List<Buff> BuffBehaviour;

    //TEMP ��ʱ��¼
    public MSG msg;
    //��ɫ�Ļ������� ������ ����id ��� XXX��
    public CharacterVO characterData;
    //TODO����һ����Ҫ�� �����ƶ� �����Ǳ������ʩ��E������תʱ����������ROOTMOTION�Ķ�����ô���ܿ��������ı��ɫ ϣ����ʱ�����ɳ��������������ƶ�
    public bool KeepMove;
    public bool canRotate = true;

    public List<GameObject> weapons;
    public AnimCoverData animCoverData;

    public bool IsDestroyed = false;

    public _Character(CharacterVO vo, GameObject obj, CharacterBaseInfo baseInfo = null)
    {
        characterData = vo;
        this.baseInfo = baseInfo;
        if (baseInfo == null)
        {
            this.baseInfo = new CharacterBaseInfo();
        }
        obj.SetActive(true);
        trans = obj.transform;
        anim = obj.GetComponent<Animator>();
        var cc = obj.GetComponent<CharacterController>();
        cc.enabled = false;
        InitCharacter();
        GameContext.AllCharacter.Add(this);
        msg.characterName = vo.characterName;

    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    public virtual void InitCharacter()
    {
        eventDispatcher = new _EventDispatcher();
        SkillBehaviour = new List<Skill>();
        BuffBehaviour = new List<Buff>();
        property = new Property(this);
        //if (baseInfo.needControl) physic = new PhysicController(this);
        characterAnimator = new CharacterAnimator();
        characterAnimator.Init(anim,null);
        fsm = new FSM(this);
        msg = new MSG();
        animCoverData = new AnimCoverData(this);
        //stateBar = new CommonStateBar(this);

        weapons = trans.GetComponent<Binding>().weapons;
        if (characterData.id == 99) property.hp.AddExAddValue(999999);
        AddEventListener();
    }

    public void AddEventListener()
    {

    }

    public void RemoveEventListener()
    {

    }

    private bool hadDie = false;
    /// <summary>
    /// ÿִ֡��
    /// </summary>
    public void OnUpdate()
    {

        physic?.OnUpdate();
        fsm?.OnUpdate();
        //�Ƿ�����
        if (property.IsDie())
        {
            if (!hadDie)
            {
                hadDie = true;
                physic.cc.enabled = false;
                //stateBar.Hide();
                CharacterManager.Eventer.Event(CharacterManager.CHARACTER_DIE, this);
            }
            return;
        }

/*        if (baseInfo.isNeedStateBar)
        {
            if (!stateBar.Root.activeSelf)
            {
                stateBar.Show();
            }
            stateBar.OnUpdate();
        }
        else
        {
            if (stateBar.Root.activeSelf)
            {
                stateBar.Hide();
            }
        }*/

        //msg ��Ϣ��¼
        msg.characterProperty = property.GetDesc();
        msg.curSkill = "\n";
        msg.curBuff = "\n";
        //skill��buff����
        for (int i = 0; i < SkillBehaviour.Count; i++)
        {
            var skill = SkillBehaviour[i];
            skill.OnUpdate();
            msg.curSkill += skill.GetDesc() + "\n";
        }

        for (int i = 0; i < BuffBehaviour.Count; i++)
        {
            var buff = BuffBehaviour[i];
            buff.OnUpdate();
            msg.curBuff += buff.GetDesc() + "\n";
        }
    }

    public GameObject GetWeapon(int index = 0)
    {
        return weapons[index];
    }

    public void AddSkillBehaviour(Skill skill)
    {
        SkillBehaviour.Add(skill);
        skill.OnEnter();
    }

    //��Ҫ֪���Ƴ���ԭ�� ����ʱ�䵽���Զ��Ƴ� ���߱���ѣ�δ���Ƴ� ֮���
    public void RemoveSkillBehaviour(Skill skill)
    {
        SkillBehaviour.Remove(skill);
        skill.OnExit();
    }

    /// <summary>
    /// �Ƿ��������
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public Skill GetSkill(int skillId)
    {
        for (int i = 0; i < SkillBehaviour.Count; i++)
        {
            var skill = SkillBehaviour[i];
            if (skill.skillData.Id == skillId) return skill;
        }
        return null;
    }

    public void AddBuffBehaviour(Buff buff)
    {
        BuffBehaviour.Add(buff);
        buff.OnEnter();
    }

    public void RemoveBuffBehaviour(Buff buff)
    {
        BuffBehaviour.Remove(buff);
        buff.OnExit();
    }

    public TagBuffComponent HasTagBuff(TagType tag)
    {
        for (int i = 0; i < BuffBehaviour.Count; i++)
        {
            var buff = BuffBehaviour[i];
            if (buff is BuffGroup)
            {
                TagBuffComponent tagbuff = ((BuffGroup)buff).HasTagBuff(tag);
                if (tagbuff != null)
                    return tagbuff;
            }
        }
        return null;
    }

    public void BuffAsk(BaseAction action)
    {
        for (int i = 0; i < BuffBehaviour.Count; i++)
        {
            var buff = BuffBehaviour[i];
            if (buff is BuffGroup)
            {
                ((BuffGroup)buff).BuffAsk(action);
            }
        }
    }

    public void OnDestory()
    {
        IsDestroyed = true;
        RemoveEventListener();
        //EventDispatcher.GetInstance().Event(EventDispatcher.CHARACTER_DESTORY, this);
        fsm?.OnDestory();
        physic?.OnDestory();
        physic = null;
        fsm = null;
        //������
        for (int i = 0; i < SkillBehaviour.Count; i++)
        {
            var item = SkillBehaviour[i];
            item.OnDestroy();
        }
        //����BUFF
        for (int i = 0; i < BuffBehaviour.Count; i++)
        {
            var item = BuffBehaviour[i];
            item.OnDestroy();
        }
        if (trans != null)
        {
            MonoBridge.GetInstance().DestroyOBJ(trans.gameObject);
        }
    }
}