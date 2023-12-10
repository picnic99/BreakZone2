using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CustomPlayable
{
    public class AnimClipNode : AnimNode
    {
        private AnimationClipPlayable able;
        public AnimClipNode(){}
        public AnimClipNode(PlayableGraph graph,playableAnimator animator,AnimationClip clip):base(graph,animator)
        {
            able = AnimationClipPlayable.Create(graph, clip);
            AddInput(able);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            //虽然有点奇怪 但是这样写能避免快速切换时导致的rootmotion第一帧位置异常的问题
            able.SetTime(0);
            able.SetTime(0);
            if(able.GetPlayState() != PlayState.Playing)
            {
                able.Play();
            }
        }

        public override void Disable()
        {
            base.Disable();
            able.SetTime(0);
            able.SetTime(0);
            if (able.GetPlayState() != PlayState.Paused)
            {
                able.Pause();
            }
        }

        public AnimationClipPlayable GetClipPlayable()
        {
            return able;
        }
    }
}