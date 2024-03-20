using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using StateSyncServer.LogicScripts.VirtualClient.VO.Anim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Bridge
{
    public class Animator
    {
        private Character crt => ins != null && ins is Character ? (Character)ins : null;
        private GameInstance ins { get; set; }
        public string curAnimKey = "";
        public long animTime = 0;

        private int frameCtn = 0; //? 地0帧算不算位移？？？？？
        private int maxFrameCtn = 0;
        private string curAnimName = "";
        private bool IsBlend = false;

        public AnimClipDataInfo info;
        public Animator(GameInstance ins)
        {
            this.ins = ins;
        }

        public void PlayAnim(string animName, float translateTime = 0.15f,bool needSync = true)
        {
            if(needSync)AnimManager.GetInstance().PlayAnim(crt, animName, translateTime);
            curAnimKey = animName;
            animTime = CommonUtils.GetCurTimeStamp();
            SetCurPlayAnimName(curAnimKey);
        }

        public void PlayStateAnim(StateVO state, float translateTime = 0.15f,bool needSync = true)
        {
            var animKey = crt.characterData.GetStateAnimKey(state.stateName);
            PlayAnim(animKey, translateTime,needSync);
        }

        /// <summary>
        /// 获取当前播放的动画的名称
        /// 一般一个动画key就对应一个动画名称
        /// 但是在混合动画的情况下 会存在根据输入的值不同会切换不同的动画
        /// 此时需要判断玩家的输入情况，再去找到对应实际播放的动画名称
        /// 再去查找对应的动画变换曲线 应用位移
        /// </summary>
        /// <param name="curAnimKey"></param>
        private void SetCurPlayAnimName(string curAnimKey)
        {
            AnimationVO animationVO = AnimConfiger.GetInstance().GetAnimByAnimKey(curAnimKey);
            if (animationVO != null)
            {
                if (animationVO.animation.AnimType != 2)
                {
                    curAnimName = animationVO.animation.Clips[0].AnimPath;
                    frameCtn = 0;
                    info = AnimConfiger.GetInstance().GetClipDataByName(curAnimName);
                    maxFrameCtn = info.curves.Count - 1;
                    IsBlend = false;
                }
                else
                {
                    //根据输入获取对应的动画曲线
                    IsBlend = true;
                }
            }
        }
        public void SetAnimNameByInput(Vector2 input, AnimationVO vo)
        {
            float x = input.X;
            float y = input.Y;
            if (x < 0)
            {
                x = -1;
            }
            else if (x > 0)
            {
                x = 1;
            }
            if (y < 0)
            {
                y = -1;
            }
            else if (y > 0)
            {
                y = 1;
            }
            foreach (var item in vo.animation.Clips)
            {
                if (item.X == x && item.Y == y && curAnimName != item.AnimPath)
                {
                    curAnimName = item.AnimPath;
                    frameCtn = 0;
                    info = AnimConfiger.GetInstance().GetClipDataByName(curAnimName);
                    maxFrameCtn = info.curves.Count - 1;
                    return;
                }
            }

        }
        public void Tick()
        {
            if (IsBlend)
            {
                Vector2 inputData = crt.input.InputData;
                AnimationVO animationVO = AnimConfiger.GetInstance().GetAnimByAnimKey(curAnimKey);
                if (animationVO != null)
                {
                    SetAnimNameByInput(inputData, animationVO);
                }
            }

            if (frameCtn++ <= 0) return;

            //添加位移
            Vector3 v = calculateOffset();
            crt.Trans.Translate(v);
        }

        /// <summary>
        /// 计算当前帧数相对于上一帧的偏移 
        /// </summary>
        public Vector3 calculateOffset()
        {
            if (info == null) return Vector3.Zero;
            var frame = frameCtn % (maxFrameCtn + 1);
            if (frame == 0) frame++;
            var curCurveData = info.curves[frame];
            var lastCurveData = info.curves[frame - 1];
            return curCurveData - lastCurveData;
        }
    }
}
