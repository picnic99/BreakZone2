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

/// <summary>
/// ��ɫ������
/// 
/// </summary>
public class Character{

	public bool isDebug = false;
	public CharacterState state = CharacterState.NEUTRAL;
	//���ڿ��ƽ�ɫλ��
	public Transform trans;
	//���ڲ��Ž�ɫ����
	public Animator anim;
	//��¼��ɫ������ֵ
	public Property property;
	//��Χ�����
	public RangeScan scan;
	//���������
	public PhysicController physic;

	public CharacterAnimator characterAnimator;
	//�¼�������
	public EventDispatcher eventDispatcher;
	//״̬�� ֻ�����ɫ״̬֮����л�
	public FSM fsm;
	//StateBar
	public CommonStateBar stateBar;

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

	public Character(CharacterVO vo,GameObject obj)
    {
		characterData = vo;
		obj.SetActive(true);
		trans = obj.transform;
		anim = obj.GetComponent<Animator>();
		InitCharacter();
		//TEMP CODE ----V
		Debug.Log("["+vo.characterName + "]������ս��");
		//DebugManager.Instance.AddMonitor(() => { return "bodyPosition = " + anim.bodyPosition; });
		//DebugManager.Instance.AddMonitor(() => { return "deltaPosition = " + anim.deltaPosition; });

		msg.characterName = vo.characterName;
    }

	/// <summary>
	/// ��ʼ������
	/// </summary>
	public virtual void InitCharacter()
    {
		eventDispatcher = new EventDispatcher();
		SkillBehaviour = new List<Skill>();
		BuffBehaviour = new List<Buff>();
		property = new Property(this);
		scan = new RangeScan(this);
		physic = new PhysicController(this);
		characterAnimator = new CharacterAnimator();
		characterAnimator.Init(anim,this);
		fsm = new FSM(this);
		msg = new MSG();
		animCoverData = new AnimCoverData(this);
		stateBar = new CommonStateBar(this);
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

	/// <summary>
	/// ÿִ֡��
	/// </summary>
	public void OnUpdate()
    {
		//״̬����
		var X = Input.GetAxis("Horizontal");
		var Z = Input.GetAxis("Vertical");
		anim.SetFloat("speedX", X);
		anim.SetFloat("speedZ", Z);

		physic.OnUpdate();
		fsm.OnUpdate();
		stateBar.OnUpdate();
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
			if(buff is BuffGroup)
            {
				TagBuffComponent tagbuff = ((BuffGroup)buff).HasTagBuff(tag);
				if(tagbuff != null)
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
			if(buff is BuffGroup)
            {
				((BuffGroup)buff).BuffAsk(action);
            }
        }
    }

	public void OnDestory()
    {
		RemoveEventListener();
		fsm.OnDestory();
    }
}