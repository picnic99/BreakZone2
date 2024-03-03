using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VO
{
    class BulletData
    {
        public int playerId;
        public int skillId;
        public int bulletId;
        public float durationTime;
        public float curDurationTime;
        public Vector3 startPos;
        public Vector3 range;
        public float moveSpeed;
        public float rot;
        public Vector3 pos;

        public BulletData(int playerId, int skillId, int bulletId, float durationTime, float curDurationTime, Vector3 startPos, Vector3 range, float moveSpeed, float rot)
        {
            this.playerId = playerId;
            this.skillId = skillId;
            this.bulletId = bulletId;
            this.durationTime = durationTime;
            this.curDurationTime = curDurationTime;
            this.startPos = startPos;
            this.range = range;
            this.moveSpeed = moveSpeed;
            this.rot = rot;
            this.pos = this.startPos;
        }
    }
}
