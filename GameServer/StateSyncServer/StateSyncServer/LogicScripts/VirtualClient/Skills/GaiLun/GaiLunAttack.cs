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

        public GaiLunAttack() : base()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (Character.GetSkill(SkillEnum.ZHIMINGDAJI) != null)
            {
                durationTime = 0;
                return;
            }
            int index = StageNum + 1;
            TimeManager.GetInstance().AddOnceTimer(this, skillData.GetFrontTime(StageNum), () =>
              {
                  var ins = new GaiLunAtkInstance(this, index, DoDamage);
                  //instanceList.Add(ins);
                  AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "atk", character.playerId, character.instance.InstanceId);
              });
        }

        public void DoDamage(Character target)
        {
            if (target.fsm.InState(StateType.Die)) return;

            AddState(target, character, StateType.Injure);
            DoDamage(target, character.property.Atk);

            var dir = character.instance.trans.Forward.Normalize();
            dir.Y = 0;
            target.physic.Move(dir * 1f, 0.2f);
            character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });
        }
    }

    class GaiLunAtkInstance : SkillInstance
    {
        public GameInstance curAtk;

        Action<Character> call;
        public GaiLunAtkInstance(Skill skill, int index, Action<Character> call)
        {
            RootSkill = skill;
            instancePath = "GaiLunAtk";
            durationTime = 1f;
            maxTriggerTarget = 99;
            IsEndRemoveObj = false;
            this.call = call;


            /*            curAtk = instanceObj.transform.Find("atk" + index).gameObject;
                        curAtk.SetActive(true);
                        Init();*/
        }

        public override void OnEnterTrigger(Character col)
        {
            base.OnEnterTrigger(col);
            Vector3 v = RootSkill.character.Trans.Position;
            GameInstance ins = InstanceManager.GetInstance().CreateEffectInstance("Common/BloodEffect", v, 0);
            TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
            {
                InstanceManager.GetInstance().RemoveInstance(ins);
            });
        }

        public override void InvokeEnterTrigger(Character target)
        {
            call.Invoke(target);
        }

        public override void InitTransform()
        {
            Vector3 pos = RootSkill.character.Trans.Position + RootSkill.character.Trans.Forward * 1f;
            float rot = RootSkill.character.Trans.Rot;
            InstanceManager.GetInstance().CreateSkillInstance(instancePath, pos, rot);
        }

        public override void AddBehaviour()
        {
            TimeManager.GetInstance().AddLoopTimer(this, () =>
            {
                if (durationTime <= 0)
                {
                    End();
                    return;
                }
                durationTime -= 0.02f;
            }, 0.02f);
        }
    }
}