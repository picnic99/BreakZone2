﻿using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Client.Net.PB.Enum;
using Assets.LogicScripts.Utils;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client
{
    class InputManager : Manager<InputManager>
    {
        public float thresholdMove = 0f;
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

/*            if (GetPlayInput().magnitude >= thresholdMove && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                if (CameraManager.GetInstance().state != CameraState.MAIN) return;
                Vector3 angles = PlayerManager.GetInstance().Self.Trans.position - CameraManager.GetInstance().curCam.transform.rotation.eulerAngles;
                proto.Rot = angles.y;
            }*/

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
        }
    }
}
