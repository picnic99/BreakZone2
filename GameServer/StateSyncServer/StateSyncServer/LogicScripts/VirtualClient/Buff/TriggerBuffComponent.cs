using System;
using System.Collections.Generic;
using UnityEngine;

public enum BuffTriggerType
{
    SKILL_COUNT,//技能施法N次
    IMMEDIATE,//立刻触发
    TAKE_DAMAGE,//造成伤害
    GET_DAMAGE,//受到伤害
    PROPERTY_CHANGE,//属性值变化
    INTER_STATE,//进入状态
    OUT_STATE,//离开状态
    INTERVAL_TIME,//间隔N秒
}

public enum BuffEndType
{
    NEVEREND,//永不结束
    DURATIONTIME,//持续N秒
    TRIGGER_COUNT,//触发N次
}

public class TriggerBuffComponent : BuffComponent
{
    public static string BUFF_TRIGGER = "TriggerBuffComponent_BUFF_TRIGGER";

    /***
     * 净化     移除眩晕、沉默、禁锢状态以及所以的负面buff                                              立刻生效       触发N次后失效
     */

    /// <summary>
    /// buff行为
    /// </summary>
    public Action<object[]> doBuffCall;
    /// <summary>
    /// 询问回调 主要响应 action在执行前对于buff是否有提升的询问
    /// </summary>
    public Action<BaseAction> askCall;
    /// <summary>
    /// buff移除前回调
    /// </summary>
    public Action endCall;
    /// <summary>
    /// 通用参数
    /// </summary>
    public object[] commonArgs;

    /// <summary>
    /// 列了一些触发类型 和 结束类型 暂时还没用上
    /// </summary>
    public BuffTriggerType canTriggerType;
    public BuffEndType canEndType;

    /// <summary>
    /// 是否触发
    /// </summary>
    private bool isTrigger = false;
    /// <summary>
    /// 是否完成
    /// </summary>
    private bool isEnd = false;
    /// <summary>
    /// 当前的触发次数
    /// </summary>
    public int curTriggerCtn = 0;


    public TriggerBuffComponent(Behaviour from)
    {
        this.from = from;
    }

    /// <summary>
    /// 询问回调
    /// </summary>
    /// <param name="action"></param>
    public override void BuffAsk(BaseAction action)
    {
        if (askCall != null)
            askCall(action);
    }

    /// <summary>
    /// 是否满足触发条件
    /// </summary>
    /// <returns></returns>
    public virtual bool CanTrigger()
    {
        return isTrigger;
    }

    /// <summary>
    /// 添加触发条件
    /// 触发N次技能
    /// </summary>
    /// <param name="skillId">技能id</param>
    /// <param name="count">次数</param>
    public void AddTrigger_SKILL_COUNT(int skillId, int count = 1)
    {
        //当前触发的次数
        int curCount = 0;
        Action<object[]> fun = null;
        fun = (args) =>
        {
            int id = (int)args[0];
            if (skillId == id)
            {
                curCount++;
                //Debug.Log(skillId + "触发1次");
                if (curCount >= count)
                {
                    isTrigger = true;
                    from.character.eventDispatcher.Off(CharacterEvent.DO_SKILL, fun);
                }
            }
        };
        //监听技能施放
        from.character.eventDispatcher.On(CharacterEvent.DO_SKILL, fun);
    }
    /// <summary>
    /// 添加触发条件
    /// 同时监听多个技能触发N次
    /// </summary>
    /// <param name="skillId">欲监听技能id列表</param>
    /// <param name="count">次数</param>
    public void AddTrigger_SKILL_COUNT(List<int> skillIds, int count = 1)
    {
        int curCount = 0;
        Action<object[]> fun = null;
        fun = (args) =>
        {
            int id = (int)args[0];
            if (skillIds.IndexOf(id) != -1)
            {
                curCount++;
                Debug.Log(id + "触发1次");
                if (curCount >= count)
                {
                    isTrigger = true;
                    from.character.eventDispatcher.Off(CharacterEvent.DO_SKILL, fun);
                }
            }
        };
        from.character.eventDispatcher.On(CharacterEvent.DO_SKILL, fun);
    }

    /*    public void AddTrigger_GET_DAMAGE()
        {
            Action<object[]> fun = null;
            fun = (args) =>
            {
                isTrigger = true;
                character.eventDispatcher.Off(CharacterEvent.GET_DAMAGE, fun);
                commonArgs = args;
            };
            character.eventDispatcher.On(CharacterEvent.GET_DAMAGE, fun);
        }*/


    /// <summary>
    /// 触发条件
    /// 立即触发
    /// </summary>
    public void AddTrigger_IMMEDIATE()
    {
        isTrigger = true;
        OnUpdate();
    }

    /// <summary>
    /// 触发条件
    /// 造成伤害
    /// </summary>
    public void AddTrigger_TAKE_DAMAGE()
    {

    }

    /// <summary>
    /// 触发条件
    /// 属性值发生改变
    /// </summary>
    /// <param name="c">目标角色</param>
    /// <param name="type">数值类型</param>
    /// <param name="mathType">比较方式</param>
    /// <param name="value">目标数值</param>
    public void AddTrigger_PROPERTY_CHANGE(Character c, PropertyType type, MathType mathType, float value)
    {
        Action<object[]> fun = null;
        fun = (args) =>
        {
            //拿到目标数值
            PropertyValue property = c.property.GetPropertyByType(type);
            #region 数值比较
            switch (mathType)
            {
                //等于
                case MathType.EQUAL:
                    if (value <= 1)
                    {
                        if ((property.finalValue / property.baseValue) == value) isTrigger = true;
                    }
                    else if (property.finalValue == value) isTrigger = true;
                    break;
                //大于
                case MathType.MORE:
                    if (value <= 1)
                    {
                        if ((property.finalValue / property.baseValue) > value) isTrigger = true;
                    }
                    else if (property.finalValue > value) isTrigger = true;
                    break;
                //小于
                case MathType.LESS:
                    if (value <= 1)
                    {
                        if ((property.finalValue / property.baseValue) < value) isTrigger = true;
                    }
                    else if (property.finalValue < value) isTrigger = true;
                    break;
                //大于等于
                case MathType.MORE_EQUAL:
                    if (value <= 1)
                    {
                        if ((property.finalValue / property.baseValue) >= value) isTrigger = true;
                    }
                    else if (property.finalValue >= value) isTrigger = true;
                    break;
                //小于等于
                case MathType.LESS_OR_EQUAL:
                    if (value <= 1)
                    {
                        if ((property.finalValue / property.baseValue) <= value) isTrigger = true;
                    }
                    else if (property.finalValue <= value) isTrigger = true;
                    break;
                default:
                    break;
            }
            #endregion
            if (isTrigger)
            {
                c.eventDispatcher.Off(CharacterEvent.PROPERTY_CHANGE, fun);
            }
        };
        //监听数值改变
        c.eventDispatcher.On(CharacterEvent.PROPERTY_CHANGE, fun);
    }

    /// <summary>
    /// 触发条件
    /// 状态发生改变
    /// </summary>
    /// <param name="c">目标角色</param>
    /// <param name="type">欲监听的状态列表 eg：toAtk dizzy ...</param>
    public void AddTrigger_STATE_CHANGE(Character c, string[] type)
    {
        List<string> types = new List<string>(type);
        Action<object[]> fun = null;
        fun = (args) =>
        {
            State state = (State)args[0];
            if (types.IndexOf(state.stateData.stateName) != -1)
            {
                isTrigger = true;
                c.eventDispatcher.Off(CharacterEvent.ADD_STATE, fun);
            }
        };
        c.eventDispatcher.On(CharacterEvent.ADD_STATE, fun);
    }

    /// <summary>
    /// 触发条件
    /// N秒后触发
    /// </summary>
    /// <param name="durationTime">持续时间</param>
    public void AddTrigger_DURATION_TIME(float durationTime)
    {
        if(base.group != null)
        {
            if (base.durationTime < durationTime) {
                durationTime = base.durationTime - 0.01f;
            }else if(base.durationTime == durationTime)
            {
                durationTime -= 0.01f;
            }
        }
        Action fun = () => { isTrigger = true; };
        TimeManager.GetInstance().RemoveTimer(this, fun);
        TimeManager.GetInstance().AddOnceTimer(this, durationTime, fun);
    }


    /// <summary>
    /// 结束条件
    /// N秒后结束
    /// </summary>
    /// <param name="durationTime">时间</param>
    public void AddEnd_DURATION_TIME(float durationTime)
    {
        if (base.group != null)
        {
            if (base.durationTime < durationTime)
            {
                durationTime = base.durationTime - 0.01f;
            }
            else if (base.durationTime == durationTime)
            {
                durationTime -= 0.01f;
            }
        }

        Action fun = () => { isEnd = true; };
        TimeManager.GetInstance().RemoveTimer(this, fun);
        TimeManager.GetInstance().AddOnceTimer(this, durationTime, fun);
    }

    /// <summary>
    /// 结束条件
    /// 效果触发N次
    /// </summary>
    /// <param name="count"></param>
    public void AddEnd_TRIGGER_COUNT(int count = 1)
    {
        Action<object[]> fun = null;
        fun = (args) =>
        {
            if (curTriggerCtn >= count) isEnd = true;
            eventDispatcher.Off(BUFF_TRIGGER, fun);
        };
        eventDispatcher.On(BUFF_TRIGGER, fun);
    }

    /// <summary>
    /// 执行触发内容
    /// </summary>
    public virtual void DoBuff()
    {
        if (doBuffCall != null)
        {
            doBuffCall(commonArgs);
        }
    }

    /// <summary>
    /// Buff完成时回调
    /// </summary>
    protected override void OnEnd()
    {
        if (endCall != null)
        {
            endCall();
        }
        base.OnEnd();
    }

    /// <summary>
    /// 是否满足退出条件
    /// </summary>
    public bool CanEnd()
    {
        return isEnd;
    }

    /// <summary>
    /// 是否结束
    /// </summary>
    private bool isOver = false;
    public override void OnUpdate()
    {
        //buff是否结束
        if (isOver) return;
        //计时
        this.durationTime -= Time.deltaTime;
        //能否触发
        if (CanTrigger())
        {
            //执行触发行为
            DoBuff();
            //当前触发次数自增
            curTriggerCtn++;
            //抛出Buff被触发事件
            eventDispatcher.Event(BUFF_TRIGGER);
            isTrigger = false;
        }

        //能否结束
        if (CanEnd())
        {
            //执行结束
            OnEnd();
            //当前触发次数归零
            curTriggerCtn = 0;
            //标记buff结束
            isOver = true;
        }
    }

    public override BuffFlag GetFlag()
    {
        return BuffFlag.OTHER;
    }
}