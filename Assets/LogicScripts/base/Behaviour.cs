using System;
using UnityEngine;

public abstract class Behaviour
{
    public _Character character;
    public float durationTime = 0;
    /// <summary>
    /// 创建后执行
    /// </summary>
    public abstract void OnEnter();
    public virtual void OnUpdate()
    {
        durationTime -= Time.deltaTime;
        if (durationTime <= 0)
        {
            OnEnd();
        }
    }

    /// <summary>
    /// 作用 移除后的处理
    /// </summary>
    public abstract void OnExit();

    /// <summary>
    /// 作用 用于通知外部移除
    /// </summary>
    protected abstract void OnEnd();

    public void DoDamage(_Character[] targets, float value)
    {
        _ActionManager.GetInstance().AddDamageAction(character, targets, value, this);
    }

    public void DoDamage(_Character target, float value)
    {
        var targets = new _Character[] { target };
        _ActionManager.GetInstance().AddDamageAction(character, targets, value, this);
    }

    public void AddState(_Character[] targets, params object[] args)
    {
        StateManager.GetInstance().AddState(character,targets, args);
    }
    
    public void AddState(_Character target, params object[] args)
    {
        var targets = new _Character[] { target };
        StateManager.GetInstance().AddState(character, targets, args);
    }


    public void AddBuff(_Character[] targets, BuffVO vo, Action<BuffGroup> initCall,Action<object[]> endBuffCall = null)
    {
        BuffManager.GetInstance().AddBuffGroup(this, targets, vo, initCall,endBuffCall);
    }
    public void AddBuff(_Character target, BuffVO vo, Action<BuffGroup> initCall,Action<object[]> endBuffCall = null)
    {
        var targets = new _Character[] { target };
        BuffManager.GetInstance().AddBuffGroup(this, targets, vo, initCall,endBuffCall);
    }

    public void PlayAnim(string name,float translateTime = 0.15f)
    {
        if(name != "")
        {
            AnimManager.GetInstance().PlayAnim(character, name, translateTime);
        }
    }
    public void PlayAnimByState(StateVO state, float translateTime = 0.3f)
    {
        if (state != null)
        {
            AnimManager.GetInstance().PlayStateAnim(character, state, translateTime);
            //AudioManager.GetInstance().Stop(true);
            if (state.state.Type == StateType.Run)
            {
                //AudioManager.GetInstance().Play("run", true);
            }
            if (state.state.Type == StateType.Move)
            {
                //AudioManager.GetInstance().Play("move", true);
            }
        }
    }

    public void StopAnim(string name)
    {
        if(name != "")
        {
            //AnimManager.GetInstance().StopAnim(character, name);
        }
    }

    public void AddBuffToSelf(BuffVO vo, Action<BuffGroup> initCall, Action<object[]> endBuffCall = null)
    {
        BuffManager.GetInstance().AddBuffGroup(this, new _Character[] { character }, vo, initCall,endBuffCall);
    }

    public void AddCustomBuff(_Character[] targets, Type buffType, Action<object[]> endBuffCall = null)
    {
        BuffManager.GetInstance().AddCustomBuffGroup(this, targets, buffType,endBuffCall);
    }
    public void AddCustomBuffToSelf(Type buffType, Action<object[]> endBuffCall = null)
    {
        BuffManager.GetInstance().AddCustomBuffGroup(this, new _Character[] { character }, buffType,endBuffCall);
    }

    public virtual string GetDesc()
    {
        return "";
    }

}