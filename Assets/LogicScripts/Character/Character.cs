using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
	FRIEND, //队友
	ENEMY, //敌人
	NEUTRAL //中立
}

/// <summary>
/// 角色管理器
/// 
/// </summary>
public class Character{

	public bool isDebug = false;
	public CharacterState state = CharacterState.NEUTRAL;
	//用于控制角色位置
	public Transform trans;
	//用于播放角色动画
	public Animator anim;
	//记录角色属性数值
	public Property property;
	//范围检测器
	public RangeScan scan;
	//物理控制器
	public PhysicController physic;

	public CharacterAnimator characterAnimator;
	//事件管理器
	public EventDispatcher eventDispatcher;
	//状态机 只负责角色状态之间的切换
	public FSM fsm;
	//StateBar
	public CommonStateBar stateBar;

	//存储角色当前正在执行的Skill 有些脱手技能 不一定有那么快结束掉
	public List<Skill> SkillBehaviour;
	//存储角色当前正在执行的BUFF
	public List<Buff> BuffBehaviour;

	//TEMP 临时记录
	public MSG msg;
	//角色的基础数据 如名字 技能id 简介 XXX的
	public CharacterVO characterData;
	//TODO（不一定需要） 保持移动 考虑是比如盖伦施法E技能旋转时，假如我是ROOTMOTION的动画那么不能靠动画来改变角色 希望此时可以由程序继续控制玩家移动
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
		Debug.Log("["+vo.characterName + "]加入了战斗");
		//DebugManager.Instance.AddMonitor(() => { return "bodyPosition = " + anim.bodyPosition; });
		//DebugManager.Instance.AddMonitor(() => { return "deltaPosition = " + anim.deltaPosition; });

		msg.characterName = vo.characterName;
    }

	/// <summary>
	/// 初始化容器
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
	/// 每帧执行
	/// </summary>
	public void OnUpdate()
    {
		//状态更新
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

		//skill与buff更新
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

	//需要知道移除的原因 比如时间到了自动移除 或者被人眩晕打断移除 之类的
	public void RemoveSkillBehaviour(Skill skill)
    {
		SkillBehaviour.Remove(skill);
        skill.OnExit();
    }

	/// <summary>
	/// 是否包含技能
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