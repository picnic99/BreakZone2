using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Common.Value
{
    /// <summary>
    /// 数值修饰器
    /// </summary>
    public class ValueModifier<T>
    {
        //修饰值
        public T value;
        //是否启用
        public bool enable;

        public ValueModifier(T value, bool enable = true)
        {
            this.value = value;
            this.enable = enable;
        }
    }
}
