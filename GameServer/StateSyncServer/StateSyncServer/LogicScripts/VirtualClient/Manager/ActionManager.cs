using StateSyncServer.LogicScripts.VirtualClient.Actions;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    /// <summary>
    /// 动作管理器
    /// 动作 ACTION 如一次伤害动作 一次治疗动作 等
    /// 角色属性由该管理器进行管理
    /// 目前考虑执行一次伤害动作的流程为：
    /// 如盖伦使用R技能 对一个目标造成伤害
    /// 1.创建一个action 填入伤害发起者 接收者 数值
    /// 然后给该管理器 管理再每帧结束时开始顺序执行aciton列表
    /// 执行一次伤害前 先询问发起者 是否有buff或者属性能够增加该数值？ 比如我一个buff可以让R技能伤害翻倍 此时需要更改数值
    /// 然后再去问一下接收者 是否有buff或者属性能够减少该数值？ 如我的属性有30%的伤害减免，又或者我有一个buff可以免疫下一次受到的伤害
    /// 然后两者进行结合 得到最终伤害数值，写入接收者的属性中
    /// </summary>
    public class ActionManager : Manager<ActionManager>
    {
        //public Queue<BaseAction> actionQueue = new Queue<BaseAction>();
        public void AddDamageAction(Character character, Character[] targets, float value, Behaviour from)
        {
            DamageAction action = new DamageAction();
            action.Init(character, targets, value, from);
            SettleInImmediate(action);
            //actionQueue.Enqueue(action);
        }

        /// <summary>
        /// 每帧结束时进行各类数值结算
        /// <summary>
        /*    public void SettleInFreameEnd()
            {
                if (actionQueue.Count <= 0) return;
                return;
                for (int i = 0; i < actionQueue.Count; i++)
                {
                    BaseAction action = actionQueue.Dequeue();
                    action.Apply();
                }
            }*/

        /// <summary>
        /// 伤害及时结算
        /// </summary>
        public void SettleInImmediate(BaseAction action)
        {
            action.Apply();
        }
    }
}

/// </summary>