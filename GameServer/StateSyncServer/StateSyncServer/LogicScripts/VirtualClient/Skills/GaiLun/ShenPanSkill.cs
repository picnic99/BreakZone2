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
        public ShenPanSkill()
        {

        }

        public override void OnEnter()
        {
            SkillDataInfo info = new SkillDataInfo()
            {
                PlayerId = character.PlayerId,
                SkillId = skillData.Id,
                StageNum = 0,
            };
            ActionManager.GetInstance().Send_GameDoSkillNtf(info);

            //PlayAnim(skillData.GetAnimKey(0));
            base.OnEnter();
            skillDurationTime = stateDurationTime = 0.4f;
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "start", this.character.PlayerId, character.InstanceId);
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            //位移逻辑
            TimeManager.GetInstance().AddLoopTime(this, 0, Global.FixedTimerMS_10, () =>
            {
                character.Trans.Translate(new Vector3(0, 0, 5 * 0.01f));
            }, 40);

            GameInstance shenPanIns = new GameInstance();
            shenPanIns.Trans.TransformMatrix = character.Trans.TransformMatrix;
            var col = new BoxCollider(character.PlayerId, new Vector4(-1, 2, 1, 0));
            shenPanIns.SetCollider(col);

            //伤害检测逻辑
            TimeManager.GetInstance().AddLoopTime(this, 400, Global.FixedFrameTimeMS, () => {
                
                shenPanIns.Trans.Tick();
                shenPanIns.Trans.Translate(new Vector3(0, 0, 20 * 0.05f));
                shenPanIns.ColCheck();

                if (col.IsColTarget)
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 检测到" + col.checkResults.Count + "个目标");
                    foreach (var item in col.checkResults)
                    {
                        BeTrigger(item);
                    }
                }
                else
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 未检测到目标");
                }
            }, 600 / Global.FixedFrameTimeMS);
        }

        /// <summary>
        /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
        /// </summary>
        /// <param name="target"></param>
        public void BeTrigger(Character target)
        {
            //float damageValue = 50;// baseDamage[skillLevel] + 0.5f * character.property.atk;
            //DoDamage(target, damageValue);
            AddState(target, character, StateType.Injure);

            //target.physic.AtkFly(5f, 1f);
        } 

        public override string GetDesc()
        {
            return "当前技能为：审判，剩余时间为：" + skillDurationTime.ToString("F2");
        }
    }
}