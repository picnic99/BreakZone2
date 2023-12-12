﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>, Manager
{
    public bool HasSelfRole = false;

    public float thresholdMove = 0f;

    public void Init()
    {

    }

    public Vector3 GetPlayInput()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        return new Vector3(x, 0, z);
    }

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
                        StopState(StateType.Move);
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
                        StopState(StateType.Run);
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
                }
            }

            LookForward();
        }
        else
        {
            if (GameContext.CharacterIncludeState(StateType.Move))
            {
                StopState(StateType.Move);
            }
            if (GameContext.CharacterIncludeState(StateType.Run))
            {
                StopState(StateType.Run);
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
                ChangeState(GameContext.SelfRole, StateType.Roll, GameContext.GetCharacterSkillIdByIndex(2));
            }
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

    public void StopState(string state)
    {
        GameContext.SelfRole.eventDispatcher.Event(CharacterEvent.STATE_OVER, state);
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