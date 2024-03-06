using StateSyncServer.LogicScripts.VirtualClient.Value;

namespace StateSyncServer.LogicScripts.VirtualClient.Base
{
    /// <summary>
    /// 属性数值
    /// </summary>
    public class PropertyValue
    {
        public float baseValue { get; private set; }//基础伤害
        public float exAdd { get; private set; }//额外固定加成
        public float exPctAdd { get; private set; }//额外百分比加成

        public float finalValue { get; private set; }//最终数值

        public ModifierGroup<float> exAddGroup = new ModifierGroup<float>();
        public ModifierGroup<float> exPctAddGroup = new ModifierGroup<float>();

        public PropertyValue(float baseValue)
        {
            exAdd = exPctAdd = 0;
            this.baseValue = baseValue;
            UpdateValue();
        }

        public ValueModifier<float> AddExAddValue(float value)
        {
            ValueModifier<float> mod = new ValueModifier<float>(value);
            exAddGroup.AddModifier(mod);
            UpdateValue();
            return mod;
        }

        public void RemoveExAddValue(ValueModifier<float> mod)
        {
            exAddGroup.RemoveModifier(mod);
            UpdateValue();
        }

        public ValueModifier<float> AddExPctAddValue(float value)
        {
            ValueModifier<float> mod = new ValueModifier<float>(value);
            exPctAddGroup.AddModifier(mod);
            UpdateValue();
            return mod;
        }

        public void RemoveExPctAddValue(ValueModifier<float> mod)
        {
            exPctAddGroup.RemoveModifier(mod);
            UpdateValue();
        }

        public void ModModifier(ValueModifier<float> mod, float value)
        {
            if (mod != null)
            {
                mod.value = value;
                UpdateValue();
            }
        }

        /// <summary>
        /// 改变修饰器个数时 更新一下最终数值
        /// </summary>
        public virtual void UpdateValue()
        {
            finalValue = baseValue;
            exAdd = GetGroupTotalValue(exAddGroup);
            exPctAdd = GetGroupTotalValue(exPctAddGroup);
            finalValue = (baseValue + exAdd) * (1 + exPctAdd);
        }

        public virtual float GetGroupTotalValue(ModifierGroup<float> group)
        {
            float total = 0;
            foreach (var item in group.groups)
            {
                if (item.enable)
                {
                    total += item.value;
                }
            }
            return total;
        }
    }
}