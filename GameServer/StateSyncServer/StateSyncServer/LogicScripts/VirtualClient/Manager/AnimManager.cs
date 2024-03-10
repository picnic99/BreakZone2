
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Bridge;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;
namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{

    /// <summary>
    /// 动画管理器
    /// </summary>
    public class AnimManager : LogicScripts.Manager.Manager<AnimManager>
    {
        /// <summary>
        /// 为某个角色播放一个动画
        /// </summary>
        /// <param name="character"></param>
        /// <param name="v"></param>
        public void PlayAnim(Character character, string animName, float translateTime = 0.15f)
        {
            //character.anim.Play(animName, translateTime);
            //character.anim.SetTrigger(animName);
            //CommonUtils.Logout(animName);

            StateSyncServer.LogicScripts.Manager.ActionManager.GetInstance().SendAnimPlayNtf(character.playerId,0, animName,translateTime);
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
/*            var animPath = AnimConfiger.GetInstance().GetAnimByAnimKey(animKey);
            AnimationClip clip = ResourceManager.GetInstance().LoadResource<AnimationClip>("Anims/" + animPath.animation.Clips[index].AnimPath);
            if (clip != null && clip.length > 0)
            {
                len = clip.length;
            }*/
            return len;
        }
    }
}