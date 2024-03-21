using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    class BoxCollider : Collider
    {
        /// <summary>
        /// x 左上坐标X
        /// y 左上坐标Y
        /// z 右下坐标x
        /// w 右下坐标Y
        /// </summary>
        public Vector4 rect;

        public List<Character> checkResults = new List<Character>();

        public bool IsColTarget => checkResults.Count > 0;

        public BoxCollider(int playerId,Vector4 rect)
        {
            this.playerId = playerId;
            this.rect = rect;
        }

        public override void Check()
        {
            base.Check();
            checkResults.Clear();
            List<Player> players = SceneManager.GetInstance().GetPlayerInSceneByPidNoSelf(playerId);
            foreach (var p in players)
            {
                Vector3 pos = p.Crt.Trans.Position;

                GetInvertMartix();
                //将玩家的点乘上这个矩阵
                Vector3 finalPos = MatrixUtils.MulMatrixVerctor(InvertMat, pos);
                if (IsPointInsideRectangle(finalPos.X, finalPos.Z, rect.X, rect.Y, rect.Z, rect.W))
                {
                    checkResults.Add(p.Crt);
                    CommonUtils.Logout("碰撞检测检测到：" + p.playerId + "在范围内");
                }
            }

            Player player = PlayerManager.GetInstance().FindPlayer(playerId);

            Vector3 LT = new Vector3(rect.X,0,rect.Y);
            Vector3 RT = new Vector3(rect.Z,0,rect.Y);
            Vector3 LB = new Vector3(rect.X,0,rect.W);
            Vector3 RB = new Vector3(rect.Z,0,rect.W);

            LT = MatrixUtils.MulMatrixVerctor(player.Crt.Trans.TransformMatrix,LT);
            RT = MatrixUtils.MulMatrixVerctor(player.Crt.Trans.TransformMatrix, RT);
            LB = MatrixUtils.MulMatrixVerctor(player.Crt.Trans.TransformMatrix, LB);
            RB = MatrixUtils.MulMatrixVerctor(player.Crt.Trans.TransformMatrix, RB);

            ActionManager.GetInstance().Send_GameDrawBoxRangeNtf(playerId, LT, RT, LB, RB);


        }

        public static bool IsPointInsideRectangle(float px, float py, float x1, float y1, float x2, float y2)
        {
            // 检查点的坐标是否在矩形的边界内  
            return px >= x1 && px <= x2 && py >= y2 && py <= y1;
        }

        public static void Test()
        {
            var rect = new Vector4(-1,2,1,0);
            Vector3 pos = new Vector3(0, 0, 3);
            //GetInvertMartix();
            //Vector3 finalPos = MatrixUtils.MulMatrixVerctor(Matrix4x4.Identity, pos);
            if (IsPointInsideRectangle(pos.X, pos.Z, rect.X, rect.Y, rect.Z, rect.W))
            {
                CommonUtils.Logout("碰撞检测检测到玩家在范围内");
            }
            else
            {
                CommonUtils.Logout("碰撞检测未检测到");
            }
        }

    }
}
