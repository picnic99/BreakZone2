using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffGroup : Buff
{
    /// <summary>
    /// buff组件
    /// </summary>
    public List<BuffComponent> buffComponents;
    /// <summary>
    /// 初始化的回调
    /// 此处扩展init
    /// 可不继承类来定制化buff属性
    /// </summary>
    public Action<BuffGroup> initCall;
    public Action<object[]> endBuffCall;

    public BuffGroup() { }

    /// <summary>
    /// 初始化
    /// 置入必要参数
    /// 同时添加buff组件
    /// </summary>
    public virtual void Init()
    {
        durationTime = buffData.durationTime;
        buffComponents = new List<BuffComponent>();
        if (initCall != null) initCall(this);
    }

    public void AddBuffComponent(BuffComponent buff)
    {
        buff.group = this;
        buffComponents.Add(buff);
    }

    public void RemoveBuffComponent(BuffComponent buff)
    {
        buffComponents.Remove(buff);
    }

    public void BuffAsk(BaseAction action) {
        for (int i = 0; i < buffComponents.Count; i++)
        {
            var buff = buffComponents[i];
            buff.BuffAsk(action);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Init();
        foreach (var component in buffComponents)
        {
            component.OnEnter();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        foreach (var component in buffComponents)
        {
            component.OnExit();
        }
        if (endBuffCall != null) endBuffCall(null);
        OnDestory();
    }

    public override void OnUpdate()
    {
        durationTime -= Time.deltaTime;
        if (durationTime <= 0 || buffComponents == null || buffComponents.Count <= 0)
        {
            OnEnd();
            return;
        }

        for (int i = 0; i < buffComponents.Count; i++)
        {
            var component = buffComponents[i];
            component.OnUpdate();
        }
    }

    public virtual void OnDestory()
    {
        buffComponents.Clear();
        buffComponents = null;
    }


    /// <summary>
    /// 添加一个数值类型
    /// durationTime
    /// 持续时间
    /// args
    /// 格式：new PropertyBuffVO(xx),....
    /// </summary>
    /// <param name="durationTime">持续时间</param>
    /// <param name="args">
    /// 
    /// </param>
    /// <returns></returns>
    public BuffComponent AddPropertyBuff(float durationTime, params object[] args)
    {
        List<PropertyBuffVO> vos = new List<PropertyBuffVO>();
        foreach (var item in args)
        {
            PropertyBuffVO vo = (PropertyBuffVO)item;
            vos.Add(vo);
        }
        PropertyBuffComponent component = new PropertyBuffComponent(from, vos);
        component.durationTime = durationTime;
        component.character = character;
        component.group = this;
        return component;
    }

    /// <summary>
    /// 添加一个触发类型的buff
    /// 触发条件
    /// 结束条件
    /// 触发内容
    /// 理论应该能支持大部分buff 特殊buff可继续继承buffComponent
    /// </summary>
    /// <returns></returns>
    public TriggerBuffComponent AddTriggerBuff()
    {
        TriggerBuffComponent component = new TriggerBuffComponent(from);
        component.character = character;
        component.group = this;
        return component;
    }

    public TagBuffComponent AddTagBuff(TagType tag, float durationTime)
    {
        TagBuffComponent component = new TagBuffComponent(from,tag,durationTime);
        component.character = character;
        component.group = this;
        return component;
    }

    public override BuffFlag GetFlag()
    {
        return BuffFlag.OTHER;
    }

    public override string GetDesc()
    {
        return buffData.buffName + " 持续时间" + durationTime.ToString("F2") + "s";
    }

    public TagBuffComponent HasTagBuff(TagType type)
    {
        for (int i = 0; i < buffComponents.Count; i++)
        {
            var buff = buffComponents[i];
            if(buff is TagBuffComponent&&((TagBuffComponent)buff).tag == type)
            {
                return (TagBuffComponent)buff;
            }
        }
        return null;
    }
}