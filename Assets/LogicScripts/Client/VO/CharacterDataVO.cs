using System.Collections.Generic;

namespace Assets.LogicScripts.Client.VO
{
    /// <summary>
    /// 角色数据
    /// </summary>
    public class CharacterDataVO
    {
        /// <summary>
        /// 角色当前属性值
        /// </summary>
        public CharacterProperty Property { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public StateType CurState { get; set; }
        public List<BuffData> BuffList { get; set; }
        public List<SkillData> SkillList { get; set; }
    }
}