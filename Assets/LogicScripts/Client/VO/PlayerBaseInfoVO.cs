using UnityEngine;

namespace Assets.LogicScripts.Client.VO
{
    /// <summary>
    /// 玩家基础数据
    /// </summary>
    public class PlayerBaseInfoVO
    {
        /// <summary>
        /// 最后一次停留的场景
        /// </summary>
        public int lastStaySceneId;
        /// <summary>
        /// 最后一次停留的位置
        /// </summary>
        public Vector3 lastStayPos;
    }
}