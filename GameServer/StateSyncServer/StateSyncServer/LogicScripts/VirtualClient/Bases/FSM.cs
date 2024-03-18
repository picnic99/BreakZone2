
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    public class FSM
    {
        //归属角色
        public Character character;
        //状态组
        public StateGroup myState;

        //是否需要检查能否切换到下一个状态
        public bool checkStateCanChange = true;

        public object[] nextState;

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
/*            DebugManager.GetInstance().AddMonitor(() =>
            {
                if (nextState != null && nextState.Length >= 2)
                {
                    return "nextState：" + nextState[1];
                }
                return "";
            });*/
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
            character.eventDispatcher.On(CharacterEvent.CHANGE_STATE, AddNextState);

        }
        private void RemoveEventListener()
        {
            character.eventDispatcher.Off(CharacterEvent.STATE_OVER, RemoveState);
            character.eventDispatcher.Off(CharacterEvent.CHANGE_STATE, AddNextState);
        }

        public void OnUpdate()
        {
            if (myState.states.Count <= 0 && nextState == null)
            {
                AddState(StateType.Idle);
            }
            if (nextState != null && checkStateCanChange)
            {
                CommonUtils.Logout("StateHandler INVOKE");
                StateHandler(nextState);
            }

            //状态行为更新
            myState.OnUpdate();
        }

        public void AddNextState(object[] args)
        {
            nextState = args;
            checkStateCanChange = true;
            //CommonUtils.Logout("nextState == args");
        }

        private State StateHandler(object[] args)
        {
            //改变者
            Character changeFrom = (Character)args[0];
            //状态名称
            string stateName = (string)args[1];

            State finalState = null;
            switch (stateName)
            {
                case StateType.Move:
                case StateType.Run:
                    finalState = AddState(stateName);
                    break;
                case StateType.Jump:
                case StateType.DoAtk:
                case StateType.Roll:
                case StateType.DoSkill:
                    int skillId = (int)args[2];
                    //若玩家欲施法技能 需要判断当前状态组中是否含有施法状态且当前状态组可以切换到施法状态
                    if (skillId <= 0)
                    {
                        CommonUtils.Logout($"技能id输入为空 请检查！！");
                        break;
                    }
                    if (SkillManager.GetInstance().InCoolDown(skillId))
                    {
                        var skillVO = SkillConfiger.GetInstance().GetSkillById(skillId);
                        if (skillVO != null)
                        {
                            CommonUtils.Logout($"[{character.characterData.characterName}]的技能[{skillVO.SkillName}] 在冷却中...");
                        }
                        else
                        {
                            CommonUtils.Logout($"技能配置中找不到id[{skillId}]，请检查配置！");
                        }
                        break;
                    }

                    if (BaseChangeStateCheck(stateName))
                    {
                        //【再次触发某技能】 当前是否已经存在该技能 是否为再次触发？
                        var existSkill = character.GetSkill(skillId);
                        if (existSkill != null)
                        {
                            //若当前技能已存在
                            if (existSkill.skillData.IsOnlySkill() && !existSkill.CanTriggerAgain)
                            {
                                //若当前技能为唯一技能且无法再次触发
                                CommonUtils.Logout($"技能创建失败： [{existSkill.skillData.SkillName}] 是唯一技能切不允许再次触发！！！");
                                break;
                            }

                            if (existSkill.CanTriggerAgain)
                            {
                                finalState = CreateStateAndSkill(skillId, stateName, existSkill);
                                break;
                            }
                        }
                        //【常规触发技能流程】
                        //isDebug 是当时想做技能编辑器时生成的角色 该角色是不通过FSM驱动
                        if (!character.baseInfo.isDebug)
                        {
                            finalState = CreateStateAndSkill(skillId, stateName);
                        }
                    }
                    //当不满足基础状态切换条件时，再判断当前技能是否支持后摇打断？
                    else if (skillId > 0 && CanSkillBackChangeState(skillId))
                    {
                        //【触发后摇打断流程】
                        //后摇打断
                        myState.RemoveState(StateType.DoSkill);
                        //实例化一个技能
                        finalState = CreateStateAndSkill(skillId, stateName);
                    }

                    break;
                case StateType.Silence:
                case StateType.Dizzy:
                case StateType.Trap:
                    finalState = AddState(stateName, 0.8f);
                    break;
                case StateType.Injure:
                case StateType.Die:
                    finalState = AddState(stateName, 0.5f);
                    break;
                default:
                    finalState = AddState(stateName);
                    break;
            }

            if (finalState != null)
            {
                nextState = null;
                CommonUtils.Logout("nextState == NULL");
            }
            checkStateCanChange = false;
            return finalState;
        }

        /// <summary>
        /// 创建一个状态以及对应的技能
        /// </summary>
        /// <param name="skillId">技能ID</param>
        /// <param name="stateName">状态的名称</param>
        /// <param name="existSkill">是否有已存在的技能（再次触发时）</param>
        private State CreateStateAndSkill(int skillId, string stateName, Skill existSkill = null)
        {
            //实例化技能
            Skill skill;
            if (existSkill != null)
            {
                skill = existSkill;
            }
            else
            {
                skill = SkillManager.GetInstance().CreateSkill(skillId, character, stateName);
            }
            //实例化状态
            var state = AddState(stateName, skill.stateDurationTime, skill.skillData.Id);
            //
            if (existSkill != null)
            {
                skill.OnEnter();
            }
            else
            {
                skill.Character = character;
            }
            state.stateInfo = skill.skillData.Id;
            //设置状态的持续时间
            state.durationTime = skill.stateDurationTime;
            //此处可能会有隐患
            //因为Buff有时候会监听你的技能施放次数 而攻击跳跃目前都算作技能 可能导致误触发
            //而在inputManager中监听此时为重置方向
            character.eventDispatcher.Event(CharacterEvent.DO_SKILL, skill.skillData.Id);

            return state;
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
                //状态发生了改变
                StateSyncServer.LogicScripts.Manager.ActionManager.GetInstance().Send_GameSyncStateChangeNtf(character.PlayerId);
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
            checkStateCanChange = true;
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
}