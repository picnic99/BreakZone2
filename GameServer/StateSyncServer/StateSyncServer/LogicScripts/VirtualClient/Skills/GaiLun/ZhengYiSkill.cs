using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;
using System;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.GaiLun
{

    /// <summary>
    /// 正义(暂时)
    /// 玩家向前跃起召唤剑阵，击飞剑阵内的敌人后，召唤剑舞，对剑阵内的敌人造成伤害同时向中心聚拢
    /// 技能持续时间 5秒  触发后击飞剑阵范围内敌人 伤害开始时间 1s后 持续3秒 共计9段伤害
    /// 状态持续时间 2秒
    /// 主要流程：召唤剑阵 击飞敌人 开始剑舞 造成伤害以及向中间点位移
    /// </summary>
    public class ZhengYiSkill : Skill
    {
        public ZhengYiSkill()
        {
            //退出时移除施加的buff
            this.IsExitRemoveBuff = true;
        }
        public override void OnEnter()
        {
            //播放正义动画
            character.canRotate = false;
            PlayAnim(skillData.GetAnimKey(0), 0.15f, false, false);
            base.OnEnter();
            stateDurationTime = skillDurationTime = 2f;

            //第一段击飞
            GameInstance zyIns = new GameInstance();
            var col = new BoxCollider(character.PlayerId, new Vector4(-4f, 11f, 4f, 3f),10);
            TimeManager.GetInstance().AddOnceTime(this, (int)(skillData.GetFrontTime(0) * 1000) + 300, () =>
            {
                zyIns.Trans.TransformMatrix = character.Trans.TransformMatrix;
                zyIns.SetCollider(col);
                zyIns.ColCheck();
                if (col.IsColTarget)
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 检测到" + col.checkResults.Count + "个目标");
                    foreach (var item in col.checkResults)
                    {
                        BeTrigger(item);
                    }
                }
            });

            TimeManager.GetInstance().AddLoopTime(this, (int)(skillData.GetFrontTime(0) * 1000) + 1000, 300, () =>
            {
                col.triggeredList.Clear();
                zyIns.ColCheck();
                if (col.IsColTarget)
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 检测到" + col.checkResults.Count + "个目标");
                    foreach (var item in col.checkResults)
                    {
                        BeTrigger(item);
                    }
                }
            }, 9);

        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "start", this.character.PlayerId, character.InstanceId);

            new ZhenYiInstance(this, BeTrigger);
        }

        public void BeTrigger(Character target)
        {

            DoDamage(target, 25);
            AddState(target, character, StateType.Injure);

            /*            AudioManager.GetInstance().Play("sword_damage1", false);
                        var dir = (target.Trans.position - character.Trans.position).normalized;
                        dir.y = 2f;
                        target.physic.Move(dir.normalized * 0.2f, 0.1f);*/
        }

        protected override void OnEnd()
        {
            character.canRotate = true;
            base.OnEnd();
        }
    }

    class ZhenYiInstance : SkillInstance
    {
        private int maxDamageCtn = 9;//最大伤害次数

        //private ColliderHelper atkFlyCheck;
        //private ColliderHelper JianWuCheck;

        public ZhenYiInstance(Skill skill, Action<Character> call)
        {
            RootSkill = skill;
            //enterCall = call;
            prefabKey = "ZhenYi";
            durationTime = 5f;
            /*            instanceObj = ResourceManager.GetInstance().GetSkillInstance(instancePath);
                        atkFlyCheck = instanceObj.transform.Find("atkFlyCheck").GetComponent<ColliderHelper>();
                        JianWuCheck = instanceObj.transform.Find("JianWuCheck").GetComponent<ColliderHelper>();*/
            //Init();
        }

        public override void AddBehaviour()
        {
            /*            TimeManager.GetInstance().AddOnceTimer(this, 0.2f, () =>
                          {
                              atkFlyCheck.gameObject.SetActive(true);
                              atkFlyCheck.OnTriggerEnterCall += AtkFly;
                          });
                        TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
                        {
                            atkFlyCheck.OnTriggerEnterCall -= AtkFly;
                            atkFlyCheck.gameObject.SetActive(false);
                        });*/

            float totalTime = 3f;
            float cd = totalTime / maxDamageCtn;
            //JianWuCheck.OnTriggerEnterCall += FeiWuAtk;
            //剑舞
            /*            TimeManager.GetInstance().AddTimeByDurationCtn(this, totalTime, maxDamageCtn, () =>
                         {
                             JianWuCheck.gameObject.SetActive(true);
                             TimeManager.GetInstance().AddOnceTimer(this, 0.3f, () =>
                             {
                                 AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, RootSkill, "sword_dance", RootSkill.Character.playerId,InstanceId);

                             });
                             TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
                             {
                                 AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, RootSkill, "sword_dance", RootSkill.Character.playerId, InstanceId);

                             });
                             //AudioManager.GetInstance().Play("atk_1", false);
                             TimeManager.GetInstance().AddFixedFrameCall(() =>
                                 {
                                     JianWuCheck.gameObject.SetActive(false);
                                 //AudioManager.GetInstance().Play("atk_style1_5", false);
                             }, 3);
                         });*/



            //击飞
            /*            var t = TimeManager.GetInstance().AddLoopTimer(this,() =>
                                {
                                    if (durationTime <= 0)
                                    {
                                        End();
                                        return;
                                    }
                                    durationTime -= Global.FixedFrameTimeMS;
                                });

                        TimeManager.GetInstance().AddOnceTimer(this,0,()=>
                        {
                            TimeManager.GetInstance().RemoveTimer(this,t);
                        });*/

        }

        public void AtkFly(Collider col)
        {
            /*            var target = GameContext.GetCharacterByObj(col.gameObject);
                        if (target == null || target == RootSkill.character) return;
                        target.physic.AtkFly(1, 1f);*/
        }

        public void FeiWuAtk(Collider col)
        {
            /*            var target = GameContext.GetCharacterByObj(col.gameObject);
                        if (target == null || target == RootSkill.character) return;
                        target.physic.Move((instanceObj.transform.position - target.trans.position) * 0.3f, 0.5f);
                        enterCall.Invoke(target);*/
        }

        public override void InitTransform()
        {
            /*            instanceObj.transform.position = RootSkill.character.Trans.position + RootSkill.character.Trans.forward * 7f;
                        instanceObj.transform.forward = RootSkill.character.Trans.forward;*/
        }


    }
}