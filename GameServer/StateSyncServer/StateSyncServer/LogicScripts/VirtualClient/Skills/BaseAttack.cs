using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills
{
    public class BaseAttack : Skill
    {
        //再次触发最大等待时间
        public float maxWaitTime = 0.5f;
        public BaseAttack() : base()
        {
            //允许再次触发
            CanTriggerAgain = true;
            maxStageNum = 3;
        }

        public override void OnEnter()
        {
            //播放动画前 考虑动画覆盖问题
            //动画覆盖分为两种 1 状态动画 2 技能动画
            AnimCoverVO vo = character.animCoverData.GetHead(belongState);
            if (vo != null)
            {
                PlayAnim(vo.animName);
            }
            else
            {
                PlayAnim(skillData.GetAnimKey(StageNum));
            }
            base.OnEnter();

            skillDurationTime = stateDurationTime + maxWaitTime;
        }

        bool triggered = false;
        public void Atk()
        {
            triggered = false;
            character.eventDispatcher.Event(CharacterEvent.PRE_ATK);
        }

        public void DoDamage(Character target)
        {
            if (target == null || target == character) return;

            AddState(target, character, StateType.Injure);

            DoDamage(target, character.property.Atk);

            triggered = true;
            Vector3 v = character.Trans.Position;
            GameInstance ins = InstanceManager.GetInstance().CreateEffectInstance("Common/BloodEffect", v, 0);

            character.eventDispatcher.Event(CharacterEvent.ATK, new Character[] { target });

            TimeManager.GetInstance().AddOnceTimer(this, 0.5f, () =>
            {
                InstanceManager.GetInstance().RemoveInstance(ins);
            });
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            Atk();
        }

        public override void OnBack()
        {
            base.OnBack();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}