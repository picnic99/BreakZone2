using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CustomPlayable
{
    public class AnimBlendRootNode : AnimMixNode
    {
        public AnimBlendRootNode(PlayableGraph graph, playableAnimator animator) : base(graph, animator)
        {
            Init();
            DebugManager.Instance.AddMonitor(() => { return "BlendRoot CurIndex = " + curIndex; });
            DebugManager.Instance.AddMonitor(() => { return "BlendRoot TargetIndex = " + targetIndex; });
            DebugManager.Instance.AddMonitor(() => { return "BlendRoot translateTime = " + translateTime; });
        }

        /// <summary>
        /// 初始化配置的动画
        /// </summary>
        public void Init()
        {
/*            List<AnimInfoData> animInfoDatas = rootAnimtor.character.characterData.GetBlendAnimInfos();
            foreach (var data in animInfoDatas)
            {
                if (data.blendClips.Length <= 0) return;
                if (ExistAnim(data.animName)) return;
                AnimBlend2DNode able = new AnimBlend2DNode(graph, data.animKey, rootAnimtor);
                var index = AddInput(able.GetPlayable());
                able.Init();
                datas.Add(index, new AnimClipData(data.animName, able));
            }*/
        }

        public override float[] EnableTargetAnim(string targetAnimName)
        {
            int tarIndex;
            float animLen = 0.5f;

            //查找目标动画对应的input下标 若没有则新建
            var data = GetAnimClipDataIndex(targetAnimName);

            if (data != -1)
            {
                tarIndex = data;
                var animData = GetAnimClipData(tarIndex);
                animData.able.Enable();
            }
            else
            {
                AnimBlend2DNode able = new AnimBlend2DNode(graph, targetAnimName, rootAnimtor);
                tarIndex = AddInput(able.GetPlayable());
                able.Init();
                able.Enable();
                datas.Add(tarIndex, new AnimClipData(targetAnimName, able));
            }

            return new float[] { tarIndex, animLen };
        }
    }
}