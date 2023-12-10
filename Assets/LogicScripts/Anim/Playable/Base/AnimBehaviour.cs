using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CustomPlayable
{
    public class AnimBehaviour : PlayableBehaviour
    {
        private AnimNode node;


        public AnimBehaviour()
        {

        }

        public void Init(AnimNode node)
        {
            this.node = node;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            node?.Execute(playable, info);
        }
    }

}
