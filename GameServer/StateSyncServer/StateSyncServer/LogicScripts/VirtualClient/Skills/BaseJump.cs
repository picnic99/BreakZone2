using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills
{
    public class BaseJump : Skill
    {
        public BaseJump()
        {
            CanTriggerAgain = true;
        }
        public override void OnEnter()
        {
            PlayAnim(skillData.GetAnimKey(0));
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "jumpStart", character.PlayerId,this.character.InstanceId);
            float len = AnimManager.GetInstance().GetAnimTime(skillData.GetAnimKey(0));
            TimeManager.GetInstance().AddOnceTime(this, (int)(len * 1000), () =>
            {
                PlayAnim(skillData.GetAnimKey(1));
            });
            base.OnEnter();
            skillDurationTime = stateDurationTime = 99f;
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (StageNum >= 2)
            {
                CanTriggerAgain = false;
            }
            character.physic.Jump();
            /*        character.physic.Move( (GameContext.GetDirByInput(character) + Vector3.up) * 5f,0.5f);

                    TimeManager.GetInstance().AddLoopTimer(this, 0.5f, () => {
                        if (character.physic.isGround)
                        {
                            PlayAnim("endJump");
                            TimeManager.GetInstance().RemoveAllTimer(this);
                            durationTime = 0.5f;
                        }
                    });*/
            _EventDispatcher.GetInstance().On(_EventDispatcher.PLAYER_JUMPED, EndJump);
        }

        public void EndJump(object[] args)
        {
            PlayAnim(skillData.GetAnimKey(2));
            AudioEventDispatcher.GetInstance().Event(MomentType.DoSkill, this, "jumpEnd", character.PlayerId,character.InstanceId);
            //AudioManager.GetInstance().Play("jump_end", false);
            skillDurationTime = 0.5f;
            _EventDispatcher.GetInstance().Off(_EventDispatcher.PLAYER_JUMPED, EndJump);
            TimeManager.GetInstance().AddOnceTime(this, 200, () =>
            {
                //CameraManager.GetInstance().EventImpulse(0.5f);
            });
        }

        public override void OnExit()
        {
            base.OnExit();
            StopAnim(skillData.GetAnimKey(2));
            _EventDispatcher.GetInstance().Off(_EventDispatcher.PLAYER_JUMPED, EndJump);
        }
    }
}