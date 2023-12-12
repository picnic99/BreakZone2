using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>, Manager
{
    public bool HasSelfRole = false;

    public float thresholdMove = 0.3f;

    public Queue<StateEvent> inputQueue = new Queue<StateEvent>();

    public string str = "";

    public void Init()
    {
        DebugManager.Instance.AddMonitor(() => { return "input：" + str; });
    }

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
    /// 
    /// 还需要加上有效时间 比如我连续点击了20次攻击，目前会将所有的攻击都执行一边，应当记录时间 或者 最大队列数量 如 3个
    /// 切换还存在问题 如run》ATK无法切换需要等待run结束
    /// </summary>

    public void OnUpdate()
    {
        if (GameContext.SelfRole == null) return;

        if (HasSelfRole == false)
        {
            AddEventListener();
            HasSelfRole = true;
        }

        var moveDelta = GetPlayInput();
        //移动与奔跑
        if (moveDelta.magnitude >= thresholdMove && (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            if (GameContext.CharacterIncludeState(StateType.Move))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (!GameContext.CharacterIncludeState(StateType.Run))
                    {
                        ChangeState(GameContext.SelfRole, StateType.Run);
                        GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Move);
                        //StopState(StateType.Move);
                        //AddStateEvent(StateType.Run, StateType.Move);
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
                        //StopState(StateType.Run);
                        //AddStateEvent(StateType.Move, StateType.Run);

                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    ChangeState(GameContext.SelfRole, StateType.Run);
                    //AddStateEvent(StateType.Run);

                }
                else
                {
                    ChangeState(GameContext.SelfRole, StateType.Move);
                    //AddStateEvent(StateType.Move);
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
                //StopState(StateType.Move);
                //AddStateEvent(null, StateType.Move);
            }
            if (GameContext.CharacterIncludeState(StateType.Run))
            {
                GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, StateType.Run);
                //StopState(StateType.Run);
                //AddStateEvent(null, StateType.Run);

            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            //ChangeState(GameContext.SelfRole, StateType.DoAtk, GameContext.GetCharacterSkillIdByIndex(0));
            AddStateEvent(StateType.DoAtk, null, GameContext.GetCharacterSkillIdByIndex(0));

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ChangeState(GameContext.SelfRole, StateType.Jump, GameContext.GetCharacterSkillIdByIndex(1));
            AddStateEvent(StateType.Jump, null, GameContext.GetCharacterSkillIdByIndex(1));

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(3));
            AddStateEvent(StateType.DoSkill, null, GameContext.GetCharacterSkillIdByIndex(3));

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
            {
                //ChangeState(GameContext.SelfRole, StateType.Roll, GameContext.GetCharacterSkillIdByIndex(2));
                AddStateEvent(StateType.Roll, null, GameContext.GetCharacterSkillIdByIndex(2));
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(5));
            AddStateEvent(StateType.DoSkill, null, GameContext.GetCharacterSkillIdByIndex(5));

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //ChangeState(GameContext.SelfRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(6));
            AddStateEvent(StateType.DoSkill, null, GameContext.GetCharacterSkillIdByIndex(6));
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

    private void AddStateEvent(string addStateName, string removeName = null, params object[] args)
    {
        var arr = inputQueue.ToArray();

        if (addStateName == StateType.Move || addStateName == StateType.Run)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if(arr[i].addStateName == addStateName)
                {
                    return;
                }
            }
        }
        inputQueue.Enqueue(new StateEvent() { addStateName = addStateName, removeName = removeName, args = args });
        str = "";
        foreach (var a in arr)
        {
            str += a.addStateName + " ";
        }

        if (GameContext.CharacterIncludeState(StateType.Idle))
        {
            InvokeNextStateEvent();
        }
    }

    public void InvokeNextStateEvent()
    {
        if(inputQueue.Count <= 0)
        {
            Debug.LogWarning("输入队列为空！");
            return;
        }

        StateEvent stateEvent = inputQueue.Dequeue();

        var arr = inputQueue.ToArray();
        str = "";
        foreach (var a in arr)
        {
            str += a.addStateName + " ";
        }

        if (stateEvent.addStateName != null && stateEvent.addStateName != string.Empty)
        {
            int len = stateEvent.args.Length + 1;
            object[] objs = new object[len];
            objs[0] = stateEvent.addStateName;
            for (int i = 0; i < stateEvent.args.Length; i++)
            {
                objs[i + 1] = stateEvent.args[i];
            }
            ChangeState(GameContext.SelfRole, objs);
        }
        if (stateEvent.removeName != null && stateEvent.removeName != string.Empty)
        {
            StopState(stateEvent.removeName);
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

    public void StopState(string state)
    {
        GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, state);
    }

    public void AddEventListener()
    {
        GameContext.SelfRole.eventDispatcher.On(CharacterEvent.DO_SKILL, OnDoSkill);
        GameContext.SelfRole.eventDispatcher.On(CharacterEvent.STATE_OVER, OnStateOver);

    }

    public void RemoveEventListener()
    {
        GameContext.SelfRole.eventDispatcher.On(CharacterEvent.DO_SKILL, OnDoSkill);
        GameContext.SelfRole.eventDispatcher.On(CharacterEvent.STATE_OVER, OnStateOver);
    }

    private void OnStateOver(object[] args)
    {
        //当前队列是否为空
        if (inputQueue.Count <= 0)
        {
            //为空则切换到默认状态
            ChangeState(GameContext.SelfRole, StateType.Idle);
        }
        else
        {
            //不为空则取队列状态切换
            InvokeNextStateEvent();
        }
    }

    private void OnDoSkill(object[] args)
    {
        int id = (int)args[0];
        LookForward();
    }
}