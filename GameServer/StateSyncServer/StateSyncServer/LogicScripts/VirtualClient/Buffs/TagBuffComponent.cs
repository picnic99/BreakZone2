using StateSyncServer.LogicScripts.VirtualClient.Bases;

namespace StateSyncServer.LogicScripts.VirtualClient.Buffs
{
    /// <summary>
    /// 标记类型
    /// </summary>
    public enum TagType
    {
        FASHUYONGDONG,//法术涌动
    }

    /// <summary>
    /// 标记型BUFF组件
    /// </summary>
    public class TagBuffComponent : BuffComponent
    {
        /// <summary>
        /// 标记
        /// </summary>
        public TagType tag;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">来源</param>
        /// <param name="tag">标记</param>
        /// <param name="durationTime">持续时间</param>
        public TagBuffComponent(Behaviour from, TagType tag, float durationTime)
        {
            this.from = from;
            this.tag = tag;
            this.durationTime = durationTime;
        }

        public override BuffFlag GetFlag()
        {
            return BuffFlag.OTHER;
        }
    }
}