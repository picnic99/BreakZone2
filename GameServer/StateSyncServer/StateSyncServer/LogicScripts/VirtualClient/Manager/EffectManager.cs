using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using StateSyncServer.LogicScripts.VO;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{

    /// <summary>
    /// 特效管理器
    /// 负责特效的创建与回收
    /// 如施法技能时附带的特效 角色身上的特效 各种特效
    /// </summary>
    public class EffectManager : Manager<EffectManager>
    {
        public int curEffectIdIndex = 1;

        //private Dictionary<int, GameEffect> datas = new Dictionary<int, GameEffect>();

        /// <summary>
        /// 创建特效
        /// </summary>
        public void CreateEffect()
        {

        }
        
        /// <summary>
        /// 变换特效
        /// </summary>
        /// <param name="instanceId"></param>
        public void TransformEffect(int instanceId)
        {

        }

        /// <summary>
        /// 移除特效
        /// </summary>
        public void RemoveEffect(int instanceId)
        {

        }
    }
}