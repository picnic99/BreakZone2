using Assets.LogicScripts.Client.Enum;
using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Client.Net.Protocols;
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

        public PlayerOptEnum GetKeyOptState(KeyCode key)
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

        public PlayerOptEnum GetMouseOptState(int mouseKey)
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
            if (!GameContext.IsCrtReceiveInput) return;

            if (GameContext.CurRole == null) return;

            //前进 后退 左移 右移 加速 攻击 跳跃 技能1 技能2 技能3 技能4 瞄准 闪避
            GamePlayerOptReq proto = new GamePlayerOptReq();
            proto.input = GetPlayInput();
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

            if(GetPlayInput().magnitude >= thresholdMove && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                if (CameraManager.GetInstance().state != CameraState.MAIN) return;
                Vector3 angles = GameContext.CurRole.trans.position - CameraManager.GetInstance().curCam.transform.rotation.eulerAngles;
                proto.Rot = angles.y;
            }

            NetManager.GetInstance().SendProtocol(proto);
        }
    }
}
