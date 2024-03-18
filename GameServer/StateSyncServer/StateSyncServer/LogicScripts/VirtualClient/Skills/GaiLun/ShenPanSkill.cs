﻿using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
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
            PlayAnim(skillData.GetAnimKey(0));
            base.OnEnter();
            skillDurationTime = stateDurationTime = 0.6f;
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "start", this.character.PlayerId, character.InstanceId);

        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            new ShenPanInstance(this, BeTrigger);
        }

        /// <summary>
        /// 被触发 超负荷在飞行过程中碰到敌人后会调用这里
        /// </summary>
        /// <param name="target"></param>
        public void BeTrigger(Character target)
        {
            float damageValue = 50;// baseDamage[skillLevel] + 0.5f * character.property.atk;
            DoDamage(target, damageValue);
            target.physic.AtkFly(5f, 1f);
        }

        protected override void OnEnd()
        {
            character.KeepMove = false;
            base.OnEnd();
        }

        public override string GetDesc()
        {
            return "当前技能为：审判，剩余时间为：" + skillDurationTime.ToString("F2");
        }
    }

    class ShenPanInstance : SkillInstance
    {
        float skillDurationTime1 = 0.4f;
        float skillDurationTime2 = 0.6f;

        List<Character> triggeredTargets = new List<Character>();
        public ShenPanInstance(Skill skill, Action<Character> call)
        {
            RootSkill = skill;
            enterCall = call;
            instancePath = "ShenPan2";
            //Init();
        }

        public override void InitTransform()
        {
            Vector3 pos = RootSkill.character.Trans.Position;
            float rot = RootSkill.character.Trans.Rot;
            InstanceManager.GetInstance().CreateSkillInstance(instancePath, pos, rot);
            //挂载crt下方
        }

        public override void AddBehaviour()
        {
            var crt = RootSkill.character;
            crt.physic.Move(crt.Trans.Forward.Normalize() * 2.5f, 0.3f);
            var t = TimeManager.GetInstance().AddLoopTimer(this, () =>
            {
                if (skillDurationTime1 > 0)
                {
                    //skillDurationTime1 -= Time.deltaTime;
                }
                else if (skillDurationTime2 > 0)
                {
                    /*                    instanceObj.transform.SetParent(null);
                                        instanceObj.transform.position += instanceObj.transform.forward * Time.deltaTime * 20f;*/
                    //取消挂载crt
                    //向前位移
                }
            }, 0f);
            TimeManager.GetInstance().AddOnceTimer(this, skillDurationTime1 + skillDurationTime2, () =>
            {
                End();
                TimeManager.GetInstance().RemoveAllTimer(this);
            });
        }

        public override void InvokeEnterTrigger(Character target)
        {
            var crt = RootSkill.character;
            if (target == null || target == crt) return;
            if (triggeredTargets.IndexOf(target) != -1)
            {
                return;
            }
            triggeredTargets.Add(target);
            base.InvokeEnterTrigger(target);
        }
    }
}