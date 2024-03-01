using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class AddBulletNtf:Protocol
    {
        public BulletData bulletData;

        public AddBulletNtf(BulletData bulletData)
        {
            protocolId = (int)ProtocolId.CLENT_BULLET_ADD_NTF;
            this.bulletData = bulletData;
        }
    }
}
