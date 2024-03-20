using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    public class Collider
    {
        public int playerId;

        /// <summary>
        /// 要检查哪些类型
        /// </summary>
        public int ColliderCheckType { get; set; }
        /// <summary>
        /// 有物体进入范围时调用
        /// </summary>
        public Action<Character[]> enterCall { get; set; }
        /// <summary>
        /// 有物体停留时调用
        /// </summary>
        public Action<Character[]> stayCall { get; set; }
        /// <summary>
        /// 有物体离开范围时调用
        /// </summary>
        public Action<Character[]> exitCall { get; set; }

        protected Matrix4x4 InvertMat;


        public virtual void Check()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tL">左上</param>
        /// <param name="tR">右上</param>
        /// <param name="bL">左下</param>
        /// <param name="bR">右下</param>
        public void SetRect(Vector3 tL,Vector3 bR)
        {
            //坐标转换
            Player p = PlayerManager.GetInstance().FindPlayer(playerId);
            Matrix4x4 mat = p.Crt.Trans.TransformMatrix;
            if(Matrix4x4.Invert(mat,out Matrix4x4 matR))
            {
                //将玩家的点乘上这个矩阵
                //然后与矩形进行边界比较
            }
 /*           tL = MatrixUtils.MulMatrixVerctor(mat, tL);
            tR = MatrixUtils.MulMatrixVerctor(mat, tR);
            tL = MatrixUtils.MulMatrixVerctor(mat, tL);
            tL = MatrixUtils.MulMatrixVerctor(mat, tL);*/
        }

        protected Matrix4x4 GetInvertMartix()
        {
            Player p = PlayerManager.GetInstance().FindPlayer(playerId);
            Matrix4x4 mat = p.Crt.Trans.TransformMatrix;
            if (Matrix4x4.Invert(mat, out Matrix4x4 matR))
            {
                InvertMat = matR;
                return matR;
            }
            return matR;
        }
    }
}
