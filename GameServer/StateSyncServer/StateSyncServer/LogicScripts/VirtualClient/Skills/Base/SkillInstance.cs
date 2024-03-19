using Msg;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System;
using EventDispatcher = StateSyncServer.LogicScripts.VirtualClient.Manager.EventDispatcher;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.Base
{

    /// <summary>
    /// 技能实例 由Skill类创建
    /// 目的 协助技能的行为 如创建特效 检测目标
    /// 暂时想法是 skill 负责调度 和决定最终结果 
    /// 如何时创建什么实例 以及实例检测目标后执行什么逻辑 
    /// 击飞 击退 造成伤害恢复血量施加buff等
    /// </summary>
    public abstract class SkillInstance : GameInstance
    {
        /// <summary>
        /// 所属技能
        /// </summary>
        public Skill RootSkill { get; set; }
        /// <summary>
        /// 实例的路径
        /// </summary>
        public string prefabKey = "";
        /// <summary>
        /// 实例持续时间
        /// </summary>
        public float durationTime = 1f;
        public float maxDurationTime = 10f;
        /// <summary>
        /// 角色死亡时技能移除
        /// </summary>
        public bool IsCharacterDeadRemove;
        /// <summary>
        /// 技能是否结束
        /// </summary>
        public bool IsEnd = false;
        /// <summary>
        /// 最大触发目标数
        /// </summary>
        public int maxTriggerTarget = 99;
        /// <summary>
        /// 当前触发目标数
        /// </summary>
        protected int curTriggerTarget = 0;
        /// <summary>
        /// 是否需要碰撞器检测
        /// </summary>
        protected bool needTriggerCheck = true;
        /// <summary>
        /// 技能结束时移除物体
        /// </summary>
        protected bool IsEndRemoveObj = true;

        public virtual void Init(int checkType)
        {
            maxDurationTime = durationTime;
            InitTransform();
            AddBehaviour();
            if (needTriggerCheck)
            {
                //碰撞盒
                SetCollider();
            }
        }

        /// <summary>
        /// 设置检测器
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="TriggerType"></param>
        public virtual void SetCollider()
        {

        }

        /// <summary>
        /// 初始化实例创建的位置信息
        /// </summary>
        public abstract void InitTransform();
        /// <summary>
        /// 添加实例的行为 如移动 炸裂 旋转 等等
        /// </summary>
        public abstract void AddBehaviour();
        /// <summary>
        /// 触发时逻辑
        /// </summary>
        public virtual void OnEnterTrigger(Character target)
        {
            /*            if (target.state != collider.info.TriggerType)
                        {
                            return;
                        }*/
            curTriggerTarget++;

            InvokeEnterTrigger(target);

            if (curTriggerTarget >= maxTriggerTarget)
            {
                End();
            }
        }
        public virtual void OnStayTrigger(Collider col)
        {

        }
        public virtual void OnExitTrigger(Collider col)
        {

        }

        public virtual void InvokeEnterTrigger(Character target)
        {
            if (enterCall != null)
            {
                //enterCall.Invoke(target);
            }
        }
        public virtual void InvokeStayTrigger(Character target)
        {
            if (stayCall != null)
            {
                //stayCall.Invoke(target);
            }
        }
        public virtual void InvokeExitTrigger(Character target)
        {
            if (exitCall != null)
            {
                //exitCall.Invoke(target);
            }
        }

        public virtual void End()
        {
            IsEnd = true;
            durationTime = 0;
            /*            if (needTriggerCheck && collider != null)
                        {
                            collider.OnTriggerEnterCall -= OnEnterTrigger;
                            collider.OnTriggerStayCall -= OnStayTrigger;
                            collider.OnTriggerExitCall -= OnExitTrigger;
                            collider.SetActive(false);
                        }*/
            TimeManager.GetInstance().RemoveAllTimer(this);
            if (IsEndRemoveObj)
            {
                InstanceManager.GetInstance().RemoveInstance(this);
            }
            else
            {
                TimeManager.GetInstance().AddOnceTimer(this, maxDurationTime, () =>
                {
                    InstanceManager.GetInstance().RemoveInstance(this);
                });
            }
        }

        public void OnDestroy()
        {
            IsEnable = false;
            InstanceManager.GetInstance().RemoveInstance(this);
        }
    }
}