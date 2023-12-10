using System;
using System.Collections.Generic;
using UnityEngine;
public class FSM
{
    //归属角色
    public Character character;
    //状态组
    public StateGroup myState;

    public FSM(Character c)
    {
        character = c;
        myState = new StateGroup(character);
        Init();
        AddEventListener();
    }

    /// <summary>
    /// 初始化
    /// 目前只设置了一下默认状态
    /// </summary>
    public void Init()
    {
        var defaultState = new State(character, StateConfiger.GetInstance().GetStateByType(StateType.Idle));
        myState.SetDefault(defaultState);
    }

    private void AddEventListener()
    {
        //当状态结束时通知状态机来移除状态
        //此处为什么要有监听移除？
        //目前状态移除有两种 一种是像眩晕沉默的这种有固定时间，时间到了由state自动移除状态
        //另外一种是不知道什么时候会移除的状态 如移动状态 角色可以一直移动 直到玩家松开w时抛出事件
        //还有就是技能和攻击时，此时虽然能预测大概需要持续多久，但是会存在打断情况，比如前摇后摇时
        character.eventDispatcher.On(CharacterEvent.STATE_OVER, RemoveState);
        //改变状态时触发，此处是状态添加核心处理，主要由两种触发情况
        //1.玩家输入触发，玩家施放技能或者各种操作时 会通过这里来添加状态
        //2.技能或者buff触发，部分技能和buff在满足条件时会改变玩家状态 如眩晕 击飞 沉默 各种
        character.eventDispatcher.On(CharacterEvent.CHANGE_STATE, StateHandler);

    }
    private void RemoveEventListener()
    {
        character.eventDispatcher.Off(CharacterEvent.STATE_OVER, RemoveState);
        character.eventDispatcher.Off(CharacterEvent.CHANGE_STATE, StateHandler);
    }

    public void OnUpdate()
    {
        //状态行为更新
        myState.OnUpdate();
    }


    private void StateHandler(object[] args)
    {
        //改变者
        Character changeFrom = (Character)args[0];
        //状态名称
        string stateName = (string)args[1];

        switch (stateName)
        {
            case StateType.Move:
            case StateType.Run:
                AddState(stateName);
                break;
            case StateType.Jump:
            case StateType.DoAtk:
            case StateType.Roll:
            case StateType.DoSkill:
                int skillId = (int)args[2];
                //若玩家欲施法技能 需要判断当前状态组中是否含有施法状态且当前状态组可以切换到施法状态
                //判断一下当前是否存在该技能未结束？不允许玩家同时含有两个相同的skill实例
                if (skillId > 0 && BaseChangeStateCheck(stateName))
                {
                    //正常状态切换流程

                    //当前是否已经存在该技能 是否为再次触发？
                    var existSkill = character.GetSkill(skillId);
                    if (existSkill != null )
                    {
                        if (existSkill.skillData.IsOnlySkill() && !existSkill.CanTriggerAgain)
                        {
                            Debug.LogError($"[{existSkill.skillData.SkillName}]是唯一技能，当前已存在无法再次施放！！！");
                            return;
                        }
                        if (existSkill.CanTriggerAgain)
                        {
                            AddState(stateName, existSkill.stateDurationTime, existSkill.skillData.Id);
                            existSkill.OnEnter();
                            break;
                        }
                    }
                    if (!character.isDebug)
                    {
                        //实例化一个技能
                        if (SkillManager.GetInstance().InCoolDown(skillId))
                        {
                            Debug.LogWarning("技能" + skillId + "在冷却中");
                            break;
                        }

                        var skill = SkillManager.GetInstance().CreateSkill(skillId, character, stateName);
                        AddState(stateName, skill.stateDurationTime, skill.skillData.Id);
                        skill.Character = character;
                        character.eventDispatcher.Event(CharacterEvent.DO_SKILL, skill.skillData.Id);
                    }
                }
                else if (skillId > 0 && CanSkillBackChangeState(skillId))
                {
                    //后摇打断逻辑
                    if (SkillManager.GetInstance().InCoolDown(skillId))
                    {
                        Debug.LogWarning("技能" + skillId + "在冷却中");
                        break;
                    }
                    //后摇打断
                    myState.RemoveState(StateType.DoSkill);
                    //实例化一个技能
                    var skill = SkillManager.GetInstance().CreateSkill(skillId, character);
                    var state = AddState(stateName, skill.stateDurationTime);
                    skill.Character = character;
                    state.stateInfo = skill.skillData.Id;
                    character.eventDispatcher.Event(CharacterEvent.DO_SKILL, skill.skillData.Id);
                }

                break;
            case StateType.Silence:
            case StateType.Dizzy:
            case StateType.Trap:
                AddState(stateName, 0.8f);
                break;
            case StateType.Injure:
                AddState(stateName, 0.5f);
                break;
            default:
                AddState(stateName);
                break;
        }
    }

    /// <summary>
    /// 基础状态切换检查
    /// 1.当前状态列表 不包含 欲添加的状态
    /// 2.当前状态列表没有和待添加状态冲突的状态
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    private bool BaseChangeStateCheck(string stateName)
    {
        return !myState.IncludeState(stateName) && myState.CanChangeTo(stateName);
    }

    /// <summary>
    /// 检查当前是否满足技能后摇状态切换？
    /// 支持配置哪些技能可以打断后摇 
    /// </summary>
    /// <returns></returns>
    private bool CanSkillBackChangeState(int skillId)
    {
        if (!myState.CheckSkillBackChange()) return false;
        //读取当前状态所触发的是什么技能
        //读取可打断后摇的技能配置
        //对比一下这个技能是否满足条件
        State state = myState.GetState(StateType.DoSkill);
        if (state == null) return false;
        int skillIdForState = state.stateInfo;
        var stateSkillData = SkillConfiger.GetInstance().GetSkillById(skillIdForState);
        if (stateSkillData.GetBackBreakList() == null || stateSkillData.GetBackBreakList().Count <= 0)
        {
            return false;
        }
        return stateSkillData.GetBackBreakList().IndexOf(skillId) != -1;
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="stateName"></param>
    public State AddState(string stateName, float stateTime = 1, int stateInfo = 0)
    {
        //1.判断当前状态是否能够切换到下一个状态  多个状态的互斥状态取交集 一个不允许则不切换
        //2.切换过去之后，将该状态的互斥状态全部移除掉
        State state = myState.AddState(stateName, stateTime, stateInfo);
        if (state != null)
        {
            return state;
        }
        return null;
    }

    /// <summary>
    /// 移除状态
    /// </summary>
    /// <param name="args"></param>
    public void RemoveState(object[] args)
    {
        if (args[0].GetType() == typeof(State))
        {
            State state = args[0] as State;
            myState.RemoveState(state);
        }
        else
        {
            string state = args[0] as string;
            myState.RemoveState(state);
        }
    }


    /// <summary>
    /// 是否在该状态
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public bool InState(string stateName)
    {
        return myState.IncludeState(stateName);
    }

    public void OnDestory()
    {
        RemoveEventListener();
    }
}