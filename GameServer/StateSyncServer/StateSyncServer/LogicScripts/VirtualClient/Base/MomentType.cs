namespace StateSyncServer.LogicScripts.VirtualClient.Base
{
    public class MomentType
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        public static string EnterState => "MomentType_EnterState";
        /// <summary>
        /// 停留状态
        /// </summary>
        public static string StayState => "MomentType_StayState";
        /// <summary>
        /// 离开状态
        /// </summary>
        public static string ExitState => "MomentType_ExitState";
        /// <summary>
        /// 释放技能
        /// </summary>
        public static string DoSkill => "MomentType_DoSkill";

    }
}