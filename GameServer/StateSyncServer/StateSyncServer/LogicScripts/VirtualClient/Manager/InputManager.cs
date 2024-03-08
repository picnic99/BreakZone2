using Msg;
using StateSyncServer.LogicScripts.Net.PB.Enum;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class InputManager
    {
        public Character crt;

        public bool HasSelfRole = false;

        public float thresholdMove = 0f;

        public InputManager(Character crt)
        {
            this.crt = crt;
        }

        public void OnDestroy()
        {
            this.crt = null;
        }

        public void ApplyOpt(GamePlayerOptReq opt)
        {
            if (crt == null) return;
            Tick(opt);
        }

        public void Tick(GamePlayerOptReq opt)
        {
            Vector2 input = crt.GetPlayerInput();

            var moveDelta = input;
            //移动与奔跑
            if (moveDelta.Length() >= thresholdMove 
                && (PlayerOptEnum.IsHoldKey(opt.UpMove) 
                || PlayerOptEnum.IsHoldKey(opt.LeftMove)
                || PlayerOptEnum.IsHoldKey(opt.DownMove)
                || PlayerOptEnum.IsHoldKey(opt.RightMove)))
            {
                if (GameContext.CharacterIncludeState(StateType.Move, crt))
                {
                    if (PlayerOptEnum.IsKeyDown(opt.AddSpeed))
                    {
                        if (!GameContext.CharacterIncludeState(StateType.Run, crt))
                        {
                            ChangeState(crt, StateType.Run);
                            StopState(StateType.Move);
                        }
                    }
                }
                else if (GameContext.CharacterIncludeState(StateType.Run, crt))
                {
                    if (PlayerOptEnum.IsKeyDown(opt.AddSpeed))
                    {
                        if (!GameContext.CharacterIncludeState(StateType.Move, crt))
                        {
                            ChangeState(crt, StateType.Move);
                            StopState(StateType.Run);
                        }
                    }
                }
                else
                {
                    if (PlayerOptEnum.IsKeyDown(opt.AddSpeed))
                    {
                        ChangeState(crt, StateType.Run);

                    }
                    else
                    {
                        ChangeState(crt, StateType.Move);
                    }
                }

                if (!GameContext.CharacterIncludeState(StateType.Roll, crt))
                {
                    LookForward();
                }
            }
            else
            {
                if (GameContext.CharacterIncludeState(StateType.Move, crt))
                {
                    StopState(StateType.Move);
                }
                if (GameContext.CharacterIncludeState(StateType.Run, crt))
                {
                    StopState(StateType.Run);
                }
            }
            if (PlayerOptEnum.IsKeyDown(opt.Atk))
            {
                ChangeState(crt, StateType.DoAtk, GameContext.GetCharacterSkillIdByIndex(0,crt));

            }
            if (PlayerOptEnum.IsKeyDown(opt.Jump))
            {
                ChangeState(crt, StateType.Jump, GameContext.GetCharacterSkillIdByIndex(1,crt));
            }
            if (PlayerOptEnum.IsKeyDown(opt.Arm))
            {
                //瞄准
/*                CameraManager.GetInstance().ShowArmCam();
                crt.physic.multiply = 0.1f;*/

            }
            else if (PlayerOptEnum.IsKeyUp(opt.Arm))
            {
/*                CameraManager.GetInstance().ShowMainCam();
                crt.physic.multiply = 1f;*/
            }
            if (PlayerOptEnum.IsHoldKey(opt.Arm))
            {
                //LookForward();
            }
            if (PlayerOptEnum.IsKeyDown(opt.Flash))
            {
                if (moveDelta.Length() > 0)
                {
                    ChangeState(crt, StateType.Roll, GameContext.GetCharacterSkillIdByIndex(2, crt));
                }
            }
            if (PlayerOptEnum.IsKeyDown(opt.Skill1))
            {
                ChangeState(crt, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(3, crt));
            }
            if (PlayerOptEnum.IsKeyDown(opt.Skill2))
            {
                ChangeState(crt, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(5, crt));
            }
            if (PlayerOptEnum.IsKeyDown(opt.Skill3))
            {
                ChangeState(crt, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(6, crt));
            }
            if (PlayerOptEnum.IsKeyDown(opt.Skill4))
            {
                //ChangeState(GameContext.CurRole, StateType.DoSkill, GameContext.GetCharacterSkillIdByIndex(6));
            }
        }

        /// <summary>
        /// 看向相机方向
        /// </summary>
        private void LookForward()
        {
            float CamRot = 0;//读取客户端的相机朝向
            crt.Trans.RotateTo(CamRot);

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
            crt.eventDispatcher.Event(CharacterEvent.STATE_OVER, state);
        }
    }
}