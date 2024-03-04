using System.Collections.Generic;
using UnityEngine;

namespace Assets.LogicScripts.Client.VO
{
    /// <summary>
    /// 角色数据
    /// </summary>
    public class CharacterDataVO
    {
        /// <summary>
        /// 角色的输入值
        /// </summary>
        public Vector2 Input { get; set; }
        /// <summary>
        /// 角色当前的位置
        /// </summary>
        public Vector3 Pos { get; set; }
        /// <summary>
        /// 角色当前的旋转角度
        /// </summary>
        public float Rot { get; set; }
        /// <summary>
        /// 角色当前的状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 当前正在播放的动画
        /// </summary>
        public string animName { get; set; }
        /// <summary>
        /// 当前播放动画的时间
        /// </summary>
        public string animTime { get; set; }
        /// <summary>
        /// 角色当前属性值
        /// </summary>
        public CharacterProperty Property { get; set; }
        /// <summary>
        /// 所挂载的技能数据
        /// </summary>
        public List<BuffData> BuffList { get; set; }
        /// <summary>
        /// 所挂载的技能数据
        /// </summary>
        public List<SkillData> SkillList { get; set; }
    }
}