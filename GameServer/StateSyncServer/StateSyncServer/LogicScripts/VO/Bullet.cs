using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VO
{
    class Bullet
    {
        public BulletData data;
        public Bullet(BulletData data)
        {
            this.data = data;
        }

        int ctn = 0;
        public void Tick()
        {
            if (data.curDurationTime <= 0)
            {
                BulletManager.GetInstance().RemoveBullet(this);
                return;
            }
            data.curDurationTime -= Global.FixedFrameTimeS;
            //物体位移同时做碰撞检测
            //获取朝向
            Vector3 forward = new Vector3((float)Math.Sin(data.rot * (Math.PI / 180)), 0, (float)Math.Cos(data.rot * (Math.PI / 180)));

            data.pos.X += forward.X * data.moveSpeed * Global.FixedFrameTimeS;
            data.pos.Y += forward.Y * data.moveSpeed * Global.FixedFrameTimeS;
            data.pos.Z += forward.Z * data.moveSpeed * Global.FixedFrameTimeS;
            CommonUtils.Logout(++ctn + "子弹的位置:" + data.pos.X + "," + data.pos.Y + "," + data.pos.Z);
            //碰撞检测
        }

    }
}
