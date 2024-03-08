
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VO
{
    class PlayerGameData
    {
        //角色Id
        public int playerId = 0;
        //所在房间Id
        public int roomId = 0;
        //状态
        //public StateEnum state = StateEnum.IDLE;
        //可视范围
        public float lookRange = 100;
        //当前位置信息
        public Vector3 pos = Vector3.Zero;
        //当前旋转信息
        public float rot = 0;
        //移动速度
        public float moveSpeed = 1; //1m/s
    }
}
