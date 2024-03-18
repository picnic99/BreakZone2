
using Assets.LogicScripts.Client.Common;
using Assets.LogicScripts.Client.Manager;
using UnityEngine;

namespace Assets.LogicScripts.Client
{
    class InputManager : Manager<InputManager>
    {
        public bool HasSelfRole = false;

        public float thresholdMove = 0f;

        public Vector3 GetPlayInput()
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            return new Vector3(x, 0, z);
        }


        public bool IsValidInput(Vector3 input)
        {
            if (IsInitInput)
            {
                IsInitInput = false;
                return true;
            }
            if (lastInput != input)
            {
                return true;
            }
            return false;
        }
        public bool IsInitInput = true;
        public Vector3 lastInput = Vector3.zero;
        public float lastRot = -1;
        public override void OnUpdate()
        {
            if (!Global.InGameScene || PlayerManager.GetInstance().Self.Trans == null)
            {
                return;
            }

            var moveDelta = GetPlayInput();
            bool rotChange = false;
            if (GetPlayInput().magnitude >= thresholdMove && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                var camRot = CameraManager.GetInstance().curCam.transform.rotation.eulerAngles.y;
                var selfRot = Global.Self.Trans.rotation.eulerAngles.y;

                if (selfRot != camRot)
                {
                    rotChange = true;
                }
            }
            if (IsValidInput(moveDelta) || rotChange)
            {
                lastInput = moveDelta;
                var rot = CameraManager.GetInstance().curCam.transform.rotation.eulerAngles.y;
                Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().Send_GamePlayerInputCmdReq(moveDelta.x, moveDelta.z, rot);
            }


            //移动与奔跑
            if (moveDelta.magnitude >= thresholdMove && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                if (GameContext.CharacterIncludeState(StateType.Move))
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        if (!GameContext.CharacterIncludeState(StateType.Run))
                        {
                            ChangeState(StateType.Run);
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
                            ChangeState(StateType.Move);
                            StopState(StateType.Run);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        ChangeState(StateType.Run);

                    }
                    else
                    {
                        ChangeState(StateType.Move);
                    }
                }

                if (!GameContext.CharacterIncludeState(StateType.Roll))
                {
                    LookForward();
                }
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
                ChangeState(StateType.DoAtk, GameContext.GetCharacterSkillIdByIndex(0));

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(StateType.Jump, GameContext.GetCharacterSkillIdByIndex(1));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeState(StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(3));
            }
            if (Input.GetMouseButtonDown(1))
            {
                //瞄准
                CameraManager.GetInstance().ShowArmCam();
                GameContext.CurRole.physic.multiply = 0.1f;

            }
            else if (Input.GetMouseButtonUp(1))
            {
                CameraManager.GetInstance().ShowMainCam();
                GameContext.CurRole.physic.multiply = 1f;
            }
            if (Input.GetMouseButton(1))
            {
                //LookForward();
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (moveDelta.magnitude > 0)
                {
                    ChangeState(StateType.Roll, GameContext.GetCharacterSkillIdByIndex(2));
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeState(StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(5));
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeState(StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(6));
            }
        }


        private void LookForward()
        {
            return;
            if (CameraManager.GetInstance().state != CameraState.MAIN) return;
            Vector3 dir = PlayerManager.GetInstance().Self.Trans.position - CameraManager.GetInstance().curCam.transform.position;
            if (GameContext.CurRole.canRotate)
            {
                GameContext.CurRole.trans.forward = new Vector3(dir.x, 0, dir.z);
            }
        }

        /// <summary>
        /// 改变状态
        /// args [来源(Character),状态名称(string),技能id(int)]
        /// </summary>
        /// <param name="character"></param>
        /// <param name="args"></param>
        public void ChangeState(params object[] args)
        {
            string cmd = (string)args[0];
            int param = 0;
            if (args.Length >= 2)
            {
                param = (int)args[1];
                param += 1;
            }

            Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().Send_GamePlayerOptCmdReq(cmd, 1, param);

            //character.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, objs);
        }

        public void StopState(string state)
        {

            Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().Send_GamePlayerOptCmdReq(state, 2, 0);

            //GameContext.CurRole?.eventDispatcher.Event(CharacterEvent.STATE_OVER, state);
        }

















































        /*        public float thresholdMove = 0f;
                public bool HasSelfRole = false;

                public Vector3 GetPlayInput()
                {
                    var x = Input.GetAxis("Horizontal");
                    var z = Input.GetAxis("Vertical");
                    return new Vector3(x, 0, z);
                }

                public int GetKeyOptState(KeyCode key)
                {
                    if (Input.GetKeyDown(key))
                    {
                        return PlayerOptEnum.DOWN;
                    }
                    if (Input.GetKey(key))
                    {
                        return PlayerOptEnum.DOWN_HOLD;
                    }
                    if (Input.GetKeyUp(key))
                    {
                        return PlayerOptEnum.UP;
                    }
                    return PlayerOptEnum.UNKNOW;
                }

                public int GetMouseOptState(int mouseKey)
                {
                    if (Input.GetMouseButtonDown(mouseKey))
                    {
                        return PlayerOptEnum.DOWN;
                    }
                    if (Input.GetMouseButton(mouseKey))
                    {
                        return PlayerOptEnum.DOWN_HOLD;
                    }
                    if (Input.GetMouseButtonUp(mouseKey))
                    {
                        return PlayerOptEnum.UP;
                    }
                    return PlayerOptEnum.UNKNOW;
                }

                public override void OnUpdate()
                {
                    //if (!GameContext.IsCrtReceiveInput) return;

                    //if (GameContext.CurRole == null) return;

                    if (!PlayerManager.GetInstance().SelfIsExist) return;

                    if (!GameContext.CurScene.IsFightScene) return;

                    //if (!Input.anyKey) return;

                    var input = GetPlayInput();
                    Assets.LogicScripts.Client.Manager.ActionManager.GetInstance().SendPlayerInput(input.x,input.z);

                    //前进 后退 左移 右移 加速 攻击 跳跃 技能1 技能2 技能3 技能4 瞄准 闪避
                    GamePlayerOptReq proto = new GamePlayerOptReq();

                    proto.UpMove = GetKeyOptState(KeyCode.W);
                    proto.DownMove = GetKeyOptState(KeyCode.S);
                    proto.LeftMove = GetKeyOptState(KeyCode.A);
                    proto.RightMove = GetKeyOptState(KeyCode.D);
                    proto.AddSpeed = GetKeyOptState(KeyCode.LeftShift);
                    proto.Arm = GetMouseOptState(1);
                    proto.Flash = GetKeyOptState(KeyCode.LeftControl);
                    proto.Jump = GetKeyOptState(KeyCode.Space);
                    proto.Atk = GetMouseOptState(0);
                    proto.Skill1 = GetKeyOptState(KeyCode.Q);
                    proto.Skill2 = GetKeyOptState(KeyCode.E);
                    proto.Skill3 = GetKeyOptState(KeyCode.R);
                    proto.Skill4 = GetKeyOptState(KeyCode.F);

        *//*            if (GetPlayInput().magnitude >= thresholdMove && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
                    {
                        if (CameraManager.GetInstance().state != CameraState.MAIN) return;
                        Vector3 angles = PlayerManager.GetInstance().Self.Trans.position - CameraManager.GetInstance().curCam.transform.rotation.eulerAngles;
                        proto.Rot = angles.y;
                    }*//*

                    if (IsVaildOpt(proto))
                    {
                        NetManager.GetInstance().SendProtocol(proto);
                        lastOpt = proto;
                    }
                }


                public GamePlayerOptReq lastOpt;
                public bool IsVaildOpt(GamePlayerOptReq opt)
                {
                    if(lastOpt == null)
                    {
                        return true;
                    }
                    if(opt.UpMove != lastOpt.UpMove 
                       || opt.DownMove != lastOpt.DownMove
                       || opt.LeftMove != lastOpt.LeftMove
                       || opt.RightMove != lastOpt.RightMove
                       || opt.AddSpeed != lastOpt.AddSpeed
                       || opt.Arm != lastOpt.Arm
                       || opt.Flash != lastOpt.Flash
                       || opt.Jump != lastOpt.Jump
                       || opt.Atk != lastOpt.Atk
                       || opt.Skill1 != lastOpt.Skill1
                       || opt.Skill2 != lastOpt.Skill2
                       || opt.Skill3 != lastOpt.Skill3
                       || opt.Skill4 != lastOpt.Skill4
                       || opt.Rot != lastOpt.Rot
                      )
                    {
                        return true;
                    }

                    return false;
                }*/
    }
}
