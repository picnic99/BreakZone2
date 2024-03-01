
using Assets.LogicScripts.Client.Enum;
using UnityEngine;

namespace Assets.LogicScripts.Client.Net.Protocols
{
    class PlayerLoginRep : Protocol
    {
        /// <summary>
        /// 0 请求失败
        /// 1 成功
        /// </summary>
        public int result;
        /// <summary>
        /// 0 注销
        /// 1 登录
        /// </summary>
        public int type;
    }
}
