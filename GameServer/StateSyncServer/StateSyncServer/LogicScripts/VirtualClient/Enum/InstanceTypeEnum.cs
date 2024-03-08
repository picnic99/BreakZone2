using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Enum
{
    /// <summary>
    /// 实体ID的开头
    /// 如角色实体的ID为 10000 + 实际的id
    /// </summary>
    class InstanceTypeEnum
    {
        public static int CHARACTER = 1;
        public static int SKILL = 2;
        public static int EFFECT = 3;
        public static int OTHER = 4;
    }
}
