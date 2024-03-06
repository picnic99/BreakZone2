using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.VO
{
    public class StateVO
    {
        public cfg.State state;
        /**
         * 如何理解预测退出？
         * 当我处于idle、move、run状态 我无法预制我什么时候离开 也无法动态告诉我预计多久会离开该状态 此时属于UNKONW_EXIT
         * 当我处于atk、skill状态时，在切换状态前就应当能预料我需要多久完成该状态，如atk1时我需要0.5s执行动作，atk2时需要1s执行动作，这些是可以预测退出时间的 TIME_EXIT
         * 当处于jump状态时，玩家跳跃的高度或者中途可能落于某处无法预料，因此此类状态属于UNKOWN_EXIT
         * 当处于dizzy状态时，本应该是能够定时预测退出的，但是其也能够通过左右按键来减少退出时间，因此属于 CHANGE_TIME_EXIT
         */

        /// <summary>
        /// 状态id
        /// </summary>
        public int id { get { return state.Id; } set { } }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string stateName { get { return state.Type; } set { } }
        /// <summary>
        /// 状态进入动画
        /// </summary>
        public string stateAnimName { get { return state.AnimKey; } set { } }
        /// <summary>
        /// 状态退出类型
        /// </summary>
        public cfg.StateExitType exitType { get { return state.ExitType; } set { } }
        /// <summary>
        /// 状态优先级
        /// </summary>
        public int order { get { return state.Order; } set { } }
        /// <summary>
        /// 是否属于技能
        /// </summary>
        public bool isSkill { get { return state.IsSkill; } set { } }

        public StateVO()
        {

        }

        /// <summary>
        /// 获取状态的互斥表
        /// </summary>
        /// <returns></returns>
        public List<int> GetMutexList()
        {
            return state.MutexState;
        }
    }
}