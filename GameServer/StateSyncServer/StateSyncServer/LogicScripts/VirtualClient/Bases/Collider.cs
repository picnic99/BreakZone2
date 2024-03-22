using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bridge;
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

        public Transform Trans;
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

        public List<GameInstance> triggeredList = new List<GameInstance>();

        /// <summary>
        /// 目标最大检测次数
        /// </summary>
        public int targetMaxTriggerNum = 1;

        public Collider(int playerId , int targetMaxTriggerNum)
        {
            this.playerId = playerId;
            this.targetMaxTriggerNum = targetMaxTriggerNum;
        }


        public virtual void Check()
        {
        }

        protected Matrix4x4 GetInvertMartix()
        {
            Matrix4x4 mat = Trans.TransformMatrix;
            if (Matrix4x4.Invert(mat, out Matrix4x4 matR))
            {
                InvertMat = matR;
                return matR;
            }
            return matR;
        }
    }
}
