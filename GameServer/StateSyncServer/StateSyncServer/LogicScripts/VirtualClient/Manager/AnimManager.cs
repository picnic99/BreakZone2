using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Entity.Characters;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    /// <summary>
    /// 动画管理器
    /// 先考虑用animator 后续改用playable
    /// </summary>
    public class AnimManager : Manager<AnimManager>
    {
        /// <summary>
        /// 为某个角色播放一个动画
        /// </summary>
        /// <param name="character"></param>
        /// <param name="v"></param>
        public void PlayAnim(Character character, string animName, float translateTime = 0.15f)
        {
            character.characterAnimator.Play(animName, translateTime);
            //character.anim.SetTrigger(animName);
            //Debug.Log(animName);
        }

        public void PlayAnim(playableAnimator anim, string animName, float translateTime = 0.15f)
        {
            anim.Play(animName, translateTime);
        }

        public void PlayStateAnim(Character character, StateVO state, float translateTime = 0.15f)
        {
            //查找是否有状态动画覆盖
            var animKey = character.characterData.GetStateAnimKey(state.stateName);
            PlayAnim(character, animKey, translateTime);
        }

        public void StopAnim(Character character, string animName)
        {
            //character.anim.ResetTrigger(animName);
        }

        /// <summary>
        /// 获取某个动画的时长
        /// </summary>
        /// <param name="animName"></param>
        /// <returns></returns>
        public float GetAnimTime(string animKey, int index = 0)
        {
            float len = 0;
            var animPath = AnimConfiger.GetInstance().GetAnimByAnimKey(animKey);
            AnimationClip clip = ResourceManager.GetInstance().LoadResource<AnimationClip>("Anims/" + animPath.animation.Clips[index].AnimPath);
            if (clip != null && clip.length > 0)
            {
                len = clip.length;
            }
            return len;
        }
    }
}
