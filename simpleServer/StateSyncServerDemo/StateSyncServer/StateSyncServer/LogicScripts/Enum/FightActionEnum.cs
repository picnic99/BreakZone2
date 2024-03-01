using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Enum
{
    enum FightActionEnum
    {
        START_MOVE = 0,//开始移动
        END_MOVE = 1,//停止移动
        ROTATE = 2,//旋转
        SKILL = 3,//施放技能
    }
}
