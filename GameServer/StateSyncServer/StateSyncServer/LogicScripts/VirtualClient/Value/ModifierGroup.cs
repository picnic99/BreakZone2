using System;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Value
{

    /// <summary>
    /// 数值修饰器组
    /// </summary>
    public class ModifierGroup<T>
    {
        public List<ValueModifier<T>> groups = new List<ValueModifier<T>>();

        public void AddModifier(ValueModifier<T> mod)
        {
            groups.Add(mod);
        }

        public void RemoveModifier(ValueModifier<T> mod)
        {
            groups.Remove(mod);
        }

        /// <summary>
        /// 获取最终数值
        /// </summary>
        /// <returns></returns>
        /*    public T GetTotalValue(Action<>)
            {
                T total ;
                foreach (var item in groups)
                {
                    if (item.enable)
                    {
                        total += item.value;
                    }
                }
                return total;
            }*/
    }
}