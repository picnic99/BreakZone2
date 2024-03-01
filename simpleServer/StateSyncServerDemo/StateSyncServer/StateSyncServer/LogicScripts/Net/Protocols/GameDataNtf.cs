using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class GameDataNtf : Protocol
    {
        public int roomId;
        public List<PlayerGameData> datas;
        public List<BulletData> bulletDatas;

        public GameDataNtf(int roomId, List<PlayerGameData> datas, List<BulletData> bulletDatas)
        {
            protocolId = (int)ProtocolId.CLENT_GAME_DATA_NTF;
            this.roomId = roomId;
            this.datas = datas;
            this.bulletDatas = bulletDatas;
        }
    }
}
