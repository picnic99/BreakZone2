using Msg;
using StateSyncServer.LogicScripts.Net.PB.Enum;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using StateSyncServer.LogicScripts.VirtualClient.States;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class InputController
    {
        private Character crt;

        public Vector2 InputData => crt != null ? crt.Player.Input : Vector2.Zero;

        public InputController(Character crt)
        {
            this.crt = crt;
        }

        public void OnDestroy()
        {
            this.crt = null;
        }

        public void ApplyInput(GamePlayerInputCmdReq req)
        {
            if(InputData.Length() > 0)
            {
                LookForward(req.Rot);
            }
        }

        public void ApplyOpt(GamePlayerOptCmdReq req)
        {
            if(req.Type == 1)
            {
                //添加状态
                ChangeState(crt,req.Cmd,req.Param - 1);
            }
            else
            {
                //移除状态
                StopState(req.Cmd);
            }
        }

        /// <summary>
        /// 看向相机方向
        /// </summary>
        private void LookForward(float rot)
        {
            crt.Trans.Rotate(rot);
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