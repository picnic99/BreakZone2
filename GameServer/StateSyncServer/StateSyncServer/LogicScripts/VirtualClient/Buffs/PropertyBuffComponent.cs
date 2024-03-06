using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Value;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Buffs
{
    /// <summary>
    /// 数值类BUFFVO
    /// </summary>
    public class PropertyBuffVO
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        public PropertyType propertyType;
        /// <summary>
        /// 是否百分比
        /// </summary>
        public bool isPct;
        /// <summary>
        /// 数值
        /// </summary>
        public float value;

        public PropertyBuffVO(PropertyType propertyType, bool isPct, float value)
        {
            this.propertyType = propertyType;
            this.isPct = isPct;
            this.value = value;
        }
    }

    /// <summary>
    /// 数值型BUFF组件
    /// 只对character property数值做修改
    /// </summary>
    public class PropertyBuffComponent : BuffComponent
    {
        public List<PropertyBuffVO> vos;
        public List<ValueModifier<float>> mods;
        public PropertyBuffComponent(Behaviour from, List<PropertyBuffVO> vos)
        {
            this.from = from;
            this.vos = vos;
            mods = new List<ValueModifier<float>>();
        }

        public override void OnEnter()
        {
            foreach (var item in vos)
            {
                var mod = character.property.AddValue(item);
                mods.Add(mod);
            }
        }

        public override void OnExit()
        {
            for (int i = 0; i < vos.Count; i++)
            {
                PropertyBuffVO vo = vos[i];
                ValueModifier<float> mod = mods[i];
                character.property.RemoveValue(vo, mod);
            }
        }

        public override BuffFlag GetFlag()
        {
            return BuffFlag.OTHER;
        }
    }
}