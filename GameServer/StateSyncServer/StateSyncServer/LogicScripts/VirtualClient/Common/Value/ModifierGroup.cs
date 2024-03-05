using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Common.Value
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
    }
}
