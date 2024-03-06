using StateSyncServer.LogicScripts.Util;
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
        //变换矩阵
        private Matrix4x4 TransformMatrix = Matrix4x4.Identity;
        //旋转矩阵
        private Matrix4x4 RotateMatrix = Matrix4x4.Identity;
        private Vector3 position;
        private Quaternion rotation;

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


        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="rot"></param>
        public void RotateTo(float rot)
        {

        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="pos"></param>
        public void Translate(Vector3 pos)
        {

        }
    }
}
