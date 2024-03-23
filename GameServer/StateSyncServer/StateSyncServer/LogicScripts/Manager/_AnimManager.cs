using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class _AnimManager
    {
        //跳跃动画的位移数据 TODO：旋转数据 如 ROOT_POS ROOT_ROTATE
        //在播放动画时根据这些数据以及动画播放速度 能够推测出 角色的实际位移量 有精度要求
        //static JumpAnimData = [{ x: 0.00, y: 0.00, z: 0.00 }, { x: 0.00, y: 0.09, z: 0.05 }, { x: 0.00, y: 0.32, z: 0.18 }, { x: 0.00, y: 0.66, z: 0.38 }, { x: 0.00, y: 1.06, z: 0.64 }, { x: 0.00, y: 1.51, z: 0.94 }, { x: 0.00, y: 1.95, z: 1.26 }, { x: 0.00, y: 2.35, z: 1.59 }, { x: 0.00, y: 2.69, z: 1.92 }, { x: 0.00, y: 2.92, z: 2.23 }, { x: 0.00, y: 3.00, z: 2.50 }, { x: 0.00, y: 2.92, z: 2.77 }, { x: 0.00, y: 2.69, z: 3.08 }, { x: 0.00, y: 2.35, z: 3.41 }, { x: 0.00, y: 1.95, z: 3.74 }, { x: 0.00, y: 1.51, z: 4.06 }, { x: 0.00, y: 1.06, z: 4.36 }, { x: 0.00, y: 0.66, z: 4.62 }, { x: 0.00, y: 0.32, z: 4.82 }, { x: 0.00, y: 0.09, z: 4.95 }, { x: 0.00, y: 0.00, z: 5.00 }];
        public static Vector3[] JumpAnimData = new Vector3[] {
            new Vector3(0.03f,0,0.25f),
            new Vector3(0.13f,0,0.25f),
            new Vector3(-0.04f,0,0.26f),
            new Vector3(-0.05f,0,0.61f),
            new Vector3(0.07f,0,0.70f),
            new Vector3(-0.05f,0,0.74f),
            new Vector3(0.01f,0,1.09f),
            new Vector3(0.11f,0,1.09f),
            new Vector3(-0.11f,0,1.38f),
            new Vector3(-0.01f,0,1.60f),
            new Vector3(0.08f,0,1.61f) };
        //获取动画任意帧的变换数据
        public static Vector3 GetAnimAnyTimePos(Vector3[] animData, int ctn)
        {
            return animData[ctn];
        }
    }
}
