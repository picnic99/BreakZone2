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
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.GaiLun
{
    public class GaiLunAttack : BaseAttack
    {
        public string effectPath = "";
        public GameInstance atkInstance;
        public GaiLunAttack() : base()
        {

        }

        public override void OnEnter()
        {
            SkillDataInfo info = new SkillDataInfo()
            {
                PlayerId = character.PlayerId,
                SkillId = skillData.Id,
                StageNum = this.StageNum,

            };
            ActionManager.GetInstance().Send_GameDoSkillNtf(info);
            base.OnEnter();
            if (Character.GetSkill(SkillEnum.ZHIMINGDAJI) != null)
            {
                durationTime = 0;
                return;
            }
            //int index = StageNum + 1;
            TimeManager.GetInstance().AddOnceTime(this, (int)(skillData.GetFrontTime(StageNum)*1000), () =>
              {
                  atkInstance = new GaiLunAtkInstance(this, StageNum, DoDamage);
                  //instanceList.Add(ins);
                  AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "atk", character.PlayerId, character.InstanceId);
              });
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            CommonUtils.Logout(character.PlayerId + "触发ATK 开始检测");
            //开始范围检测
            if (atkInstance != null)
            {
                var col = new BoxCollider(character.PlayerId , new Vector4(-1,2,1,0));
                atkInstance.SetCollider(col);
                atkInstance.ColCheck();
                if (col.IsColTarget)
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 检测到"+ col.checkResults .Count+ "个目标");
                    foreach (var item in col.checkResults)
                    {
                        DoAtkDamage(item);
                    }
                }
                else
                {
                    CommonUtils.Logout(character.PlayerId + " ATK 未检测到目标");
                }
            }
        }

        public void DoAtkDamage(Character target)
        {
            if (target.fsm.InState(StateType.Die)) return;

            AddState(target, character, StateType.Injure);
            DoDamage(target, character.property.Atk);

            CommonUtils.Logout(target.PlayerId + "收到ATK伤害");

            //var dir = character.Trans.Forward.Normalize();
            //dir.Y = 0;
            //target.physic.Move(dir * 1f, 0.2f);
            //character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });
        }
    }

    class GaiLunAtkInstance : SkillInstance
    {
        public GameInstance curAtk;
        public int stageNum;
        Action<Character> call;
        public GaiLunAtkInstance(Skill skill, int index, Action<Character> call)
        {
            _playerId = skill.character.PlayerId;
            RootSkill = skill;
            prefabKey = "GaiLunAtk";
            durationTime = 1f;
            maxTriggerTarget = 99;
            IsEndRemoveObj = false;
            this.call = call;
            this.stageNum = index;

            //SetGameInstanceInfo();
            //SendCreateAction();
        }

        public override void SetGameInstanceInfo()
        {
            base.SetGameInstanceInfo();
            Vector3 pos = RootSkill.character.Trans.Position + RootSkill.character.Trans.Forward * 1f;
            float rot = RootSkill.character.Trans.Rot;
            var temp = new GameInstaneInfo()
            {
                PlayerId = this.PlayerId,
                Parent = 0,
                InstanceId = this.InstanceId,
                FollowType = "Root",
                PrefabKey = this.prefabKey,
                Offset = new Vec3() {X= pos.X, Y=pos.Y, Z=pos.Z },
                Rot = rot,
                StageNum = this.stageNum,
                DurationTime = durationTime,
                IsAutoDestroy = true,
            };

            this.info = temp;
        }

        public override void OnEnterTrigger(Character col)
        {
/*            base.OnEnterTrigger(col);
            Vector3 v = RootSkill.character.Trans.Position;
            GameInstance ins = InstanceManager.GetInstance().CreateEffectInstance("Common/BloodEffect", v, 0);
            TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
            {
                InstanceManager.GetInstance().RemoveInstance(ins);
            });*/
        }

        public override void InvokeEnterTrigger(Character target)
        {
            call.Invoke(target);
        }

        public override void InitTransform()
        {
            Vector3 pos = RootSkill.character.Trans.Position + RootSkill.character.Trans.Forward * 1f;
            float rot = RootSkill.character.Trans.Rot;
            InstanceManager.GetInstance().CreateSkillInstance(prefabKey, pos, rot);
        }

        public override void AddBehaviour()
        {
            TimeManager.GetInstance().AddOnceTime(this, (int)(durationTime * 1000), End);
        }
    }
}