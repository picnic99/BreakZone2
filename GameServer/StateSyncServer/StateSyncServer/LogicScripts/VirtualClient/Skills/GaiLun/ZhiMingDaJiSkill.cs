//致命打击 提升移动速度 移除所有减速效果 下次攻击附带额外伤害以及沉默效果
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Buffs;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.GaiLun
{
    public class ZhiMingDaJiSkill : Skill
    {
        //private AnimCoverVO moveCover;
        private AnimCoverVO atkCover;

        public ZhiMingDaJiSkill()
        {
            skillDurationTime = 3f;
        }

        public void ShowWeaponEffect()
        {


            InstanceManager.GetInstance().CreateEffectInstance("ZhiMingDaJi_XuLi",character.Trans.Position,character.Trans.Rot);
            //AudioManager.GetInstance().Play("sword_power", false);
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "weaponFillPower",character.PlayerId,character.InstanceId);
/*            obj.transform.parent = character.GetWeapon().transform;
            obj.transform.localPosition = new Vector3(-0.084F, 0, 0.033F);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = new Vector3(0.05F, 0.05F, 0.05F);*/
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ShowWeaponEffect();
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            //移除所有的减速效果
            //BuffManager.GetInstance().RemoveBuff(character);

            //动画覆盖 更改后续的移动动画和攻击动画
            //moveCover = new AnimCoverVO("GAILUN_SWORD_RUN");
            atkCover = new AnimCoverVO("GAILUN_SWORD_HARD_ATK");

            //character.animCoverData.Add(StateType.Move, moveCover);
            character.animCoverData.Add(StateType.DoAtk, atkCover);
            AddBuff(new Character[] { character }, new BuffVO("致命打击加移速", skillDurationTime), (buff) =>
            {
                buff.AddBuffComponent(buff.AddPropertyBuff(skillDurationTime, new PropertyBuffVO(PropertyType.MOVESPEED, false, 0.35f)));
            }, (args) =>
            {
                //character.animCoverData.Remove(StateType.Move, moveCover);
                character.animCoverData.Remove(StateType.DoAtk, atkCover);
                //character.animCoverData.Remove(StateType.Run, atkCover);
            });

            //攻击施加沉默状态
            character.eventDispatcher.On(CharacterEvent.PRE_ATK, ATK_BUFF);
        }

        protected override void OnEnd()
        {
            character.eventDispatcher.Off(CharacterEvent.PRE_ATK, ATK_BUFF);
            base.OnEnd();
        }

        //添加沉默状态
        private void ATK_BUFF(object[] target)
        {
            character.animCoverData.Remove(StateType.DoAtk, atkCover);
            //character.animCoverData.Remove(StateType.Move, moveCover);

            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "hardAtk",character.PlayerId,character.InstanceId);

            new ZhiMingDaJiInstance(this, DoTrigger, 0);
            new ZhiMingDaJiInstance(this, DoTrigger, 15);
            new ZhiMingDaJiInstance(this, DoTrigger, -15);

            character.eventDispatcher.Off(CharacterEvent.PRE_ATK, ATK_BUFF);
            skillDurationTime = 0f;
        }

        public void DoTrigger(Character target)
        {
            DoDamage(new Character[] { target }, 25);
        }

        public override string GetDesc()
        {
            return "当前技能为：致命打击，剩余时间为：" + skillDurationTime.ToString("F2");
        }
    }

    public class ZhiMingDaJiInstance : SkillInstance
    {
        //飞轮有两道伤害
        List<Character> triggeredTargets = new List<Character>();

        float moveOffset = 0;
        float maxTime = 2;

        bool isBack = false;

        public ZhiMingDaJiInstance(Skill skill, Action<Character> call, float moveOffset)
        {
            RootSkill = skill;
            prefabKey = "ZhiMingDaJi";
            durationTime = maxTime;
            //enterCall = call;
            this.moveOffset = moveOffset;
            //Init();
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, RootSkill, "flywheel", RootSkill.character.PlayerId,InstanceId);
        }

        public override void AddBehaviour()
        {
            /*            TimeManager.GetInstance()._AddLoopTimer(this, () =>
                        {
                            if (durationTime <= 0)
                            {
                                if (isBack && Vector3.Distance(Trans.Position, RootSkill.character.Trans.Position) <= 0.1f)
                                {
                                    End();
                                    return;
                                }
                            }
                            DoMove();
                            durationTime -= Global.FixedFrameTimeS;
                        }, 0.25f);*/
            End();

        }

        private void DoMove()
        {
            //SetActive(true);

            // TODO 返回时追踪角色位置
/*            if (durationTime <= maxTime / 2)
            {
                if (!isBack)
                {
                    isBack = true;
                    triggeredTargets.Clear();
                }
                instanceObj.transform.position -= instanceObj.transform.forward * Time.deltaTime * 30f;
                var scale = instanceObj.transform.localScale - Vector3.one * Time.deltaTime * 3f;
                instanceObj.transform.localScale = scale.x < 0.2f ? new Vector3(0.2f, 0.2f, 0.2f) : scale;
                instanceObj.transform.forward = -(RootSkill.character.Trans.position - instanceObj.transform.position);
            }
            else
            {
                instanceObj.transform.position += instanceObj.transform.forward * Time.deltaTime * 30f;
                instanceObj.transform.localScale += Vector3.one * Time.deltaTime * 3f;
            }*/
        }



        public override void InitTransform()
        {
/*            instanceObj.transform.position = RootSkill.character.Trans.position + RootSkill.character.Trans.forward * 1f;
            instanceObj.transform.forward = RootSkill.character.Trans.forward;
            instanceObj.transform.RotateAround(instanceObj.transform.position, Vector3.up, moveOffset);*/
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