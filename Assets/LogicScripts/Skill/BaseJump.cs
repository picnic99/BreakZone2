using UnityEngine;

public class BaseJump : Skill
{
    public BaseJump()
    {
        skillDurationTime = stateDurationTime = 99f;
        CanTriggerAgain = true;
    }
    public override void OnEnter()
    {
        PlayAnim(skillData.GetAnimKey(0));
        float len = AnimManager.GetInstance().GetAnimTime2(skillData.GetAnimKey(0));
        TimeManager.GetInstance().AddOnceTimer(this, len, () => {
            PlayAnim(skillData.GetAnimKey(1));
        });
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        base.OnTrigger();
        if (StageNum >= 2)
        {
            CanTriggerAgain = false;
        }
        character.physic.Jump(10);
        /*        character.physic.Move( (GameContext.GetDirByInput(character) + Vector3.up) * 5f,0.5f);

                TimeManager.GetInstance().AddLoopTimer(this, 0.5f, () => {
                    if (character.physic.isGround)
                    {
                        PlayAnim("endJump");
                        TimeManager.GetInstance().RemoveAllTimer(this);
                        durationTime = 0.5f;
                    }
                });*/
        EventDispatcher.GetInstance().On(EventDispatcher.PLAYER_JUMPED, EndJump);
    }

    public void EndJump(object[] args)
    {
        PlayAnim(skillData.GetAnimKey(2));
        skillDurationTime = 0.5f;
        EventDispatcher.GetInstance().Off(EventDispatcher.PLAYER_JUMPED, EndJump);
        TimeManager.GetInstance().AddOnceTimer(this, 0.2f, () => {
            CameraManager.GetInstance().EventImpulse(0.5f);           
        });
    }


    protected override void EndState()
    {
        character.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Jump);
    }

    public override void OnExit()
    {
        base.OnExit();
        StopAnim(skillData.GetAnimKey(2));
        EventDispatcher.GetInstance().Off(EventDispatcher.PLAYER_JUMPED, EndJump);
    }
}