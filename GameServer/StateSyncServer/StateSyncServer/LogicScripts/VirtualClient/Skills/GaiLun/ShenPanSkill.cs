using Msg;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.GaiLun
{
    /// <summary>
    /// 审判 temp
    /// 角色挥动武器向前旋转 召唤龙卷风向前飞去
    /// 对命中的敌人造成伤害和击飞效果
    /// </summary>
    public class ShenPanSkill : Skill
    {
        /// <summary>
        /// 玩家移动时的速度
        /// </summary>
        private float crtMoveSpeed = 5f;
        /// <summary>
        /// 龙卷风移动速度
        /// </summary>
        private float ljfMoveSpeed = 20f;
        /// <summary>
        /// 龙卷风检测范围
        /// </summary>
        private Vector4 colRangeRect = new Vector4(-1, 2, 1, 0);
        /// <summary>
        /// 第一段 玩家跟随龙卷风一起移动
        /// </summary>
        private int time1 = 400;
        /// <summary>
        /// 第二段 龙卷风向前移动 玩家脱手
        /// </summary>
        private int time2 = 600;

        public ShenPanSkill()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            skillDurationTime = stateDurationTime = 0.4f;
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "start", this.character.PlayerId, character.InstanceId);
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            //位移逻辑
            TimeManager.GetInstance().AddLoopTime(this, 0, Global.FixedFrameTimeMS, () =>
            {
                character.Trans.Translate(new Vector3(0, 0, crtMoveSpeed * Global.FixedFrameTimeS));
            }, time1/ Global.FixedFrameTimeMS);

            GameInstance shenPanIns = new GameInstance();
            shenPanIns.Trans.TransformMatrix = character.Trans.TransformMatrix;
            var col = new BoxCollider(character.PlayerId, colRangeRect);
            shenPanIns.SetCollider(col);

            //伤害检测逻辑
            TimeManager.GetInstance().AddLoopTime(this, time1, Global.FixedFrameTimeMS, () => {
                shenPanIns.Trans.Tick();
                shenPanIns.Trans.Translate(new Vector3(0, 0, ljfMoveSpeed * Global.FixedFrameTimeS));
                shenPanIns.ColCheck();
                if (col.IsColTarget)
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 检测到" + col.checkResults.Count + "个目标");
                    foreach (var item in col.checkResults)
                    {
                        BeTrigger(item);
                    }
                }
            }, time2 / Global.FixedFrameTimeMS);
        }

        /// <summary>
        /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
        /// </summary>
        /// <param name="target"></param>
        public void BeTrigger(Character target)
        {
            float damageValue = 50;// baseDamage[skillLevel] + 0.5f * character.property.atk;
            DoDamage(target, damageValue);
            AddState(target, character, StateType.Injure);

            //target.physic.AtkFly(5f, 1f);
        } 

        public override string GetDesc()
        {
            return "当前技能为：审判，剩余时间为：" + skillDurationTime.ToString("F2");
        }
    }
}