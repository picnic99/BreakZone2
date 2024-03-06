using StateSyncServer.LogicScripts.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.Enum;
using StateSyncServer.LogicScripts.VirtualClient.Manager;

namespace StateSyncServer.LogicScripts.VO
{
    class Skill
    {
        public int playerId;
        public int skillId;
        public int tickCtn = 0;//当前播放帧
        public Vector3[] animData;//动画数据
        public Character player;//角色引用

        public Skill(int playerId, int skillId)
        {
            this.playerId = playerId;
            this.skillId = skillId;
            this.player = null; //CharacterManager.GetInstance().GetPlayer(this.playerId);
            if (this.skillId == (int)SkillId.XUAN_FENG_ZHAN)
            {
                this.animData = AnimManager.JumpAnimData;
            }
            else if (this.skillId == (int)SkillId.JIN_ZHUEN_DAN_MU)
            {
                this.animData = new Vector3[0];
                var data = new BulletData(this.playerId, this.skillId, 1, 10, 10, this.player.data.pos, new Vector3(2, 0.1f, 0.5f), 5, this.player.data.rot);
                var blt = new Bullet(data);
                BulletManager.GetInstance().CreateBullet(blt);
            }
        }

        public void Tick()
        {
            if (this.tickCtn >= this.animData.Length)
            {
                CommonUtils.Logout("玩家[" + this.playerId + "]技能结束 当前位移为" + this.player.data.pos.X + "," + this.player.data.pos.Y + "," + this.player.data.pos.Z);
                SkillManager.GetInstance().RemoveSkill(this);
                return;
            }
            var ctn = this.tickCtn++;
            var curOffset = AnimManager.GetAnimAnyTimePos(this.animData, ctn);
            var lastOffset = Vector3.Zero;
            if (ctn > 0)
            {
                lastOffset = AnimManager.GetAnimAnyTimePos(this.animData, ctn - 1);
            }

            var offset = new Vector3(
                curOffset.X - lastOffset.X,
                curOffset.Y - lastOffset.Y,
                curOffset.Z - lastOffset.Z
                );

            //计算相对位移
            this.player.MoveAt(offset);
        }
    }
}
