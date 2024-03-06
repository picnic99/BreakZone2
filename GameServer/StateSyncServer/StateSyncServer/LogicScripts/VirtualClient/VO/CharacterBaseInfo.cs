using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.VO
{
    public class CharacterBaseInfo
    {
        public bool isDebug = false;
        public bool canControl = false;
        public bool needControl = false;
        public bool isNeedStateBar = false;

        public static CharacterBaseInfo GetShowBaseInfo()
        {
            CharacterBaseInfo info = new CharacterBaseInfo();
            return info;
        }

        public static CharacterBaseInfo GetFightBaseInfo()
        {
            CharacterBaseInfo info = new CharacterBaseInfo();
            info.canControl = true;
            info.needControl = true;
            info.isNeedStateBar = true;
            return info;
        }
    }
}
