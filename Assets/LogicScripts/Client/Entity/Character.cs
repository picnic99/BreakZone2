using Assets.LogicScripts.Client.VO;
using UnityEngine;

namespace Assets.LogicScripts.Client.Entity
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Character
    {
        /// <summary>
        /// 具体控制的角色
        /// </summary>
        public GameObject crtObj { get; set; }
        /// <summary>
        /// 角色的动画状态机
        /// </summary>
        public CharacterAnimator characterAnimator { get; set; }
        /// <summary>
        /// 角色的数据
        /// </summary>
        public CharacterDataVO crtData { get; set; }
    }
}