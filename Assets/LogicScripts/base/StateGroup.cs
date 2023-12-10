using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态组
/// 由于状态不是单个的可能存在多种状态叠加
/// 专门创建一个类来进行管理
/// </summary>
public class StateGroup
{
    //归属角色
    private Character character;
    //状态列表
    private List<State> states = new List<State>();
    //默认状态 状态列表为空时加入列表中
    public State defaultState;

    public StateGroup(Character character)
    {
        this.character = character;
        AddEventListener();
    }

    public void AddEventListener()
    {
        character.eventDispatcher.On(CharacterEvent.ANIM_CHANGE, AnimChange);
    }

    public void RemoveEventListener()
    {
        character.eventDispatcher.Off(CharacterEvent.ANIM_CHANGE, AnimChange);
    }

    public void AnimChange(object[] args)
    {
        if (args == null || args.Length <= 0) return;
        string stateName = (string)args[0];
        if (IncludeState(stateName))
        {
            AnimCoverVO vo = character.animCoverData.GetHead(stateName);
            if (vo != null)
            {
                AnimManager.GetInstance().PlayAnim(character, vo.animName);
            }
            else
            {
                StateVO state = StateConfiger.GetInstance().GetStateByType(stateName);
                AnimManager.GetInstance().PlayStateAnim(character, state);
            }
        }
    }

    /// <summary>
    /// 设置默认状态  一般都为idle
    /// </summary>
    /// <param name="state"></param>
    public void SetDefault(State state)
    {
        defaultState = state;
    }

    /// <summary>
    /// ASK？
    /// 获取所以的附加状态 不影响基础状态称之为附加状态
    /// 这里可能不太需要 因为目前认定是完全不考虑互斥之类的状态 称之为附加状态
    /// 其加入时直接往列表中添加 不会对现有状态有任何处理
    /// 此处好像通过该状态互斥表的配置可以实现 只需要将互斥状态置空即可
    /// </summary>
    /// <returns></returns>
/*    public List<State> GetAttachStates()
    {
        List<State> result = states.FindAll((item) => { return item.stateData.isAttachState(); });
        return result;
    }*/

    /// <summary>
    /// 每帧更新一下所以状态
    /// </summary>
    public void OnUpdate()
    {
        character.msg.curState = "\n";
        if (states.Count <= 0)
        {
            Add(defaultState);
        }
        for (int i = 0; i < states.Count; i++)
        {
            var item = states[i];
            item.OnUpdate();
            character.msg.curState += item.GetDesc() + "\n";
        }
    }


    /// <summary>
    /// 能够切换到下一个状态？
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool CanChangeTo(StateVO vo)
    {

/*        if((IncludeState(StateType.DoSkill) && vo.stateName == StateType.DoSkill))
        {
            if (CheckSkillBackChange(vo.stateName))
            {
                return true;
            }
            return false;
        }*/

        for (int i = 0; i < states.Count; i++)
        {
            var item = states[i];
            if (item.stateData.GetMutexList().IndexOf(vo.id) != -1)
            {
                //特例 若互斥的状态为技能状态 则继续判断是否满足后摇强制退出
/*                if (item.stateData.stateName == StateType.DoSkill && vo.stateName != StateType.Idle && CheckSkillBackChange(vo.stateName))
                {
                    RemoveState(StateType.DoSkill);
                    return true;
                }*/
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 能够切换到下一个状态？
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool CanChangeTo(string state)
    {
        StateVO vo = StateConfiger.GetInstance().GetStateByType(state);
        return CanChangeTo(vo);
    }

    /// <summary>
    /// 检查一下技能的后摇状态
    /// 处于后摇状态的技能即使未结束状态
    /// 也可以切换到下一个互斥状态
    /// </summary>
    /// <returns></returns>
    public bool CheckSkillBackChange()
    {
        //玩家身上挂载的所以技能若都处于后摇状态则可以直接切换
        for (int i = 0; i < character.SkillBehaviour.Count; i++)
        {
            var skill = character.SkillBehaviour[i];
            if (skill.skillState != SkillInStateEnum.BACK)
            {
                return false;
            }
        }
        return true;
    }

    private void Add(State state)
    {
        states.Add(state);
        state.OnEnter();
    }

    private void Remove(State state)
    {
        states.Remove(state);
        state.OnExit();
    }

    /// <summary>
    /// 获取状态
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public State GetState(string name)
    {
        State sta = states.Find((state) => { return state.stateData.stateName == name; });
        return sta;
    }

    /// <summary>
    /// 添加一个新状态
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public State AddState(string name, float time,int stateInfo)
    {
        StateVO vo = StateConfiger.GetInstance().GetStateByType(name);
        State sta = states.Find((State state) =>
         {
             return state.stateData.id == vo.id;
         });
        if (sta != null)
        {
            Debug.LogWarning("当前已经在" + name + "状态，无法重复切换");
            return null;
        }
        if (CanChangeTo(vo))
        {
            State state = new State(character, vo, time);
            state.stateInfo = stateInfo;
            Add(state);
            //移除掉所以的互斥状态
            RemoveMutexState(state);
            SortStates();
            character.eventDispatcher.Event(CharacterEvent.ADD_STATE, state);
            return state;
        }
        return null;
    }


    /// <summary>
    /// 移除掉当前列表中与当前状态互斥的状态
    /// </summary>
    /// <param name="state"></param>
    public void RemoveMutexState(State state)
    {
        List<int> mutex = state.stateData.GetMutexList();

        //覆盖技能互斥状态
        if (state.stateData.stateName == StateType.DoSkill)
        {
            var skillid = state.stateInfo;
            var data = SkillConfiger.GetInstance().GetSkillById(skillid);
            if (data.mutexState != null)
            {
                mutex = state.stateData.GetMutexList();
            }
            //若为瞬发技能的话 不打断移动状态 因为瞬发技能无需抬手动画
            //所以会出现移动时触发技能 技能状态加入于移动互斥 移动状态移除 技能状态瞬间消失 添加如idle状态 就播放空闲的动画
            if (data.IsInstantSkill)
            {
                /*                var stateVO = StateConfiger.GetInstance().GetStateByType(StateType.Move);
                                mutex.Remove(stateVO.id);*/
                return;
            }
        }

        //如果互斥表里有 技能状态 则移除所有 未施法完毕的技能,意思为打断当前技能行为 施法完毕的不管
        if (mutex.IndexOf(StateConfiger.GetInstance().GetStateByType(StateType.DoSkill).id) != -1 && IncludeState(StateType.DoSkill))
        {
            //移除当前正在施法中的技能
            for (int i = 0; i < character.SkillBehaviour.Count; i++)
            {
                var skill = character.SkillBehaviour[i];
                if (skill.IsSkillReleaseOver() == false)
                {
                    character.RemoveSkillBehaviour(skill);
                }
            }
        }

        List<State> temp = states.FindAll((State item) =>
        {
            return mutex.IndexOf(item.stateData.id) != -1;
        });
        foreach (var item in temp)
        {
            Remove(item);
        }
    }

    /// <summary>
    /// 移除一个状态
    /// </summary>
    /// <param name="name"></param>
    public void RemoveState(string name)
    {
        List<State> temp = states.FindAll((State item) =>
        {
            return item.stateData.stateName == name;
        });
        foreach (var item in temp)
        {
            Remove(item);
        }

        SortStates();
    }

    public void RemoveState(State state)
    {
        Remove(state);
        SortStates();
    }

    /// <summary>
    /// 对状态进行排序
    /// 优先级高的排最前面
    /// 玩家只展示第一个状态的表现
    /// 如： 击飞优先级高于眩晕 玩家处于先眩晕后击飞的状态 则需要展示击飞的动画效果 当击飞状态结束 再展示眩晕的动画效果
    /// </summary>
    public void SortStates()
    {
        states.Sort((State a, State b) =>
        {
            return a.stateData.order - b.stateData.order;
        });

    }

    /// <summary>
    /// 是否包含某个状态
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IncludeState(string name)
    {
        StateVO vo = StateConfiger.GetInstance().GetStateByType(name);
        State sta = states.Find((State state) =>
        {
            return state.stateData.id == vo.id;
        });
        if (sta != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDestory()
    {
        RemoveEventListener();
    }
}