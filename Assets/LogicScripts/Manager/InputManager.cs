using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>, Manager
{
    public bool HasSelfRole = false;

    public Vector3 GetPlayInput()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        return new Vector3(x, 0, z);
    }


    /// <summary>
    /// TODO 输入队列
    /// 后续动画系统能够根据输入队列来预测玩家下一步是继续执行其它操作还是无操作切换为idle状态
    /// 主要避免玩家每次状态结束后都会切回到idle状态 此时动画会立刻切换，若是两次连续攻击可能出现卡一帧的情况
    /// 如 ATK1 Idle ATK2  应该是ATK1 ATK2 IDLE
    /// </summary>

    public void OnUpdate()
    {
        if (GameContext.SelfRole == null) return;
        if(HasSelfRole == false)
        {
            AddEventListener();
            HasSelfRole = true;
        }

        var moveDelta = GetPlayInput();
        //移动与奔跑
        if (moveDelta.magnitude > 0 && (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            if (GameContext.CharacterIncludeState(StateType.Move))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (!GameContext.CharacterIncludeState(StateType.Run))
                    {
                        ChangeState(GameContext.SelfRole, StateType.Run);
                        GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Move);
                    }
                }
            }
            else if (GameContext.CharacterIncludeState(StateType.Run))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (!GameContext.CharacterIncludeState(StateType.Move))
                    {
                        ChangeState(GameContext.SelfRole, StateType.Move);
                        GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Run);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    ChangeState(GameContext.SelfRole, StateType.Run);
                }
                else
                {
                    ChangeState(GameContext.SelfRole, StateType.Move);
                    Debug.Log("go to move");
                }
            }

            LookForward();
        }
        else
        {
            //ChangeState(GameContext.selfRole, StateType.DoSkill, 2);
            if (GameContext.CharacterIncludeState(StateType.Move))
            {
                GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Move);
            }
            if (GameContext.CharacterIncludeState(StateType.Run))
            {
                GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Run);
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            ChangeState(GameContext.SelfRole, StateType.DoAtk, GameContext.GetCharacterSkillIdByIndex(0));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(GameContext.SelfRole, StateType.Jump, GameContext.GetCharacterSkillIdByIndex(1));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(3));
        }
        if (Input.GetMouseButtonDown(1))
        {
            //ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(3));
            //瞄准
            CameraManager.GetInstance().ShowArmCam(GameContext.SelfRole);
            GameContext.SelfRole.physic.multiply = 0.1f;

        }
        else if (Input.GetMouseButtonUp(1))
        {
            CameraManager.GetInstance().ShowMainCam(GameContext.SelfRole);
            GameContext.SelfRole.physic.multiply = 1f;
        }
        if (Input.GetMouseButton(1))
        {
            //LookForward();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(moveDelta.magnitude > 0)
                ChangeState(GameContext.SelfRole, StateType.Roll, GameContext.GetCharacterSkillIdByIndex(2));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(5));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(6));
        }
    }


    private void LookForward() {
        if (CameraManager.GetInstance().state != CameraState.MAIN) return;
        Vector3 dir = GameContext.SelfRole.trans.position - CameraManager.GetInstance().curCam.transform.position;
        if (GameContext.SelfRole.canRotate)
        {
            GameContext.SelfRole.trans.forward = new Vector3(dir.x, 0, dir.z);
        }
    }

    /// <summary>
    /// 改变状态
    /// args [来源(Character),状态名称(string),技能id(int)]
    /// </summary>
    /// <param name="character"></param>
    /// <param name="args"></param>
    public void ChangeState(Character character, params object[] args)
    {
        int len = args.Length + 1;
        object[] objs = new object[len];
        objs[0] = character;
        for (int i = 0; i < args.Length; i++)
        {
            objs[i + 1] = args[i];
        }
        character.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, objs);
    }
    public void Init()
    {

    }

    public void AddEventListener()
    {
        GameContext.SelfRole.eventDispatcher.On(CharacterEvent.DO_SKILL, OnDoSkill);

    }

    public void RemoveEventListener()
    {
        GameContext.SelfRole.eventDispatcher.On(CharacterEvent.DO_SKILL, OnDoSkill);

    }


    private void OnDoSkill(object[] args)
    {
        int id = (int)args[0];
        LookForward();
    }
}