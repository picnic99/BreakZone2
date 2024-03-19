using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bridge
{
    public class Transform
    {
        public GameInstance ins;

        //变换矩阵
        private Matrix4x4 TransformMatrix = Matrix4x4.Identity;
        //旋转矩阵
        private Matrix4x4 RotateMatrix = Matrix4x4.Identity;

        private Vector3 position;
        public Vector3 Position { get => position; set => position = value; }

        private Vector3 scale;
        public Vector3 Scale { get => scale; set => scale = value; }

        private Quaternion rotation;
        public Quaternion Rotation { get => rotation; set => rotation = value; }

        private float lastRot;
        private float rot;
        public float Rot { get => rot; set => rot = value; }

        public Queue<Matrix4x4> transQueue = new Queue<Matrix4x4>();

        public Vector3 Forward
        {
            get
            {
               return MatrixUtils.MulMatrixVerctor(RotateMatrix, new Vector3(0,0,1));
            }
            private set
            {

            }
        }

        public Vector3 Right
        {
            get
            {
                return MatrixUtils.MulMatrixVerctor(RotateMatrix, new Vector3(1, 0, 0));
            }
            private set
            {

            }
        }

        public Transform(GameInstance ins)
        {
            this.ins = ins;
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="rot"></param>
        public void RotateTo(float rot)
        {
            lastRot = this.rot;
            this.Rot = rot;
            var rotChange = this.Rot - this.lastRot;
            if (rotChange != 0)
            {
                var rotMat = MatrixUtils.GetRotateYMatrix(rotChange);
                RotateMatrix = rotMat;
                transQueue.Enqueue(rotMat);
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="pos"></param>
        public void Translate(Vector3 pos)
        {
            pos.Y = 0;
            var moveMatrix = MatrixUtils.GetMoveMatrix(pos);
            transQueue.Enqueue(moveMatrix);
        }


        public void Tick()
        {
            //处理队列的变换
            while (transQueue.Count  > 0)
            {
                var mat = transQueue.Dequeue();
                this.TransformMatrix = MatrixUtils.MulMatrix(this.TransformMatrix, mat);
            }
            //处理动画的变换


            this.Position = MatrixUtils.MulMatrixVerctor(this.TransformMatrix, Vector3.Zero);
        }
    }
}
