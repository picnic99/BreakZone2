using MathNet.Numerics.LinearAlgebra.Complex;
using Msg;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VO
{
    class Character
    {
        private float lastRot = 0;
        //变换矩阵
        private Matrix4x4 TransformMatrix = Matrix4x4.Identity;

        public PlayerGameData data = new PlayerGameData();

        public Character(int playerId)
        {
            data.playerId = playerId;
        }


        /// <summary>
        /// 数据初始化
        /// 属性值 当前的状态 位置旋转 携带的技能和buff
        /// </summary>
        public void Init()
        {
            //TODO
        }

        /// <summary>
        /// 应用玩家的操作
        /// </summary>
        public void ApplyOpt(GamePlayerOptReq opt)
        {
            //todo
        }

        public void Tick()
        {
            this.Move();
        }

        public void Rotate(float rot)
        {
            this.lastRot = data.rot;
            data.rot = rot;
            var rotChange = data.rot - this.lastRot;
            if (rotChange != 0)
            {
                var rotMat = MatrixUtils.GetRotateYMatrix(rotChange);
                this.TransformMatrix = MatrixUtils.MulMatrix(this.TransformMatrix, rotMat);
            }
        }

        private void Move()
        {
            //if (data.state != StateEnum.MOVE) return;

            var moveMat = MatrixUtils.GetMoveMatrix(new Vector3(0, 0, data.moveSpeed * Global.FixedFrameTimeS));
            this.TransformMatrix = MatrixUtils.MulMatrix(this.TransformMatrix, moveMat);
            var finalPos = MatrixUtils.MulMatrixVerctor(this.TransformMatrix, new Vector3(0, 0, 0));
            data.pos = finalPos;
            CommonUtils.Logout("玩家[" + data.playerId + "]朝向" + data.rot + "度 移动至" + data.pos.X + "," + data.pos.Y + "," + data.pos.Z);

        }

        public void MoveAt(Vector3 pos)
        {
            var moveMat = MatrixUtils.GetMoveMatrix(pos);
            this.TransformMatrix = MatrixUtils.MulMatrix(this.TransformMatrix, moveMat);
            var finalPos = MatrixUtils.MulMatrixVerctor(this.TransformMatrix, new Vector3(0, 0, 0));
            data.pos = finalPos;
        }
    }
}
