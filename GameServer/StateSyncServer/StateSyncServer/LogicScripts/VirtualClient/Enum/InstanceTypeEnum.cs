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

        public static bool IsCrt(int id)
        {
            return (id / 10000) == CHARACTER;
        }
        public static bool IsSkill(int id)
        {
            return (id / 10000) == CHARACTER;
        }
        public static bool IsEffect(int id)
        {
            return (id / 10000) == CHARACTER;
        }
        public static bool IsOther(int id)
        {
            return (id / 10000) == CHARACTER;
        }

        public static int GetCrtInstanceId(int id)
        {
            return CHARACTER * 10000 + id;
        }
        public static int GetSkillInstanceId(int id)
        {
            return SKILL * 10000 + id;
        }
        public static int GetEffectInstanceId(int id)
        {
            return EFFECT * 10000 + id;
        }
        public static int GetOtherInstanceId(int id)
        {
            return OTHER * 10000 + id;
        }
    }
}
