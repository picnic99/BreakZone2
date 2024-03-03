
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class BulletManager : Manager<BulletManager>
    {
        private List<Bullet> BulletList = new List<Bullet>();

        public BulletManager()
        {

        }

        public void CreateBullet(Bullet bullet)
        {
/*            this.BulletList.Add(bullet);
            //通知房间内其它玩家 有弹道创建
            var AddBulletNtf = new AddBulletNtf(bullet.data);
            CommonUtils.Logout("玩家[" + bullet.data.playerId + "]创建弹道 [" + bullet.data.bulletId + "]");
            var p = CharacterManager.GetInstance().GetPlayer(bullet.data.playerId);*/
            //NetManager.GetInstance().SendProtoToRoom(p.data.roomId, AddBulletNtf);
        }

        public void RemoveBullet(Bullet bullet)
        {
            BulletList.Remove(bullet);
        }

        public List<Bullet> GetAllBullets()
        {
            return this.BulletList;
        }

        public void Tick()
        {
            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].Tick();
            }
        }
    }
}
