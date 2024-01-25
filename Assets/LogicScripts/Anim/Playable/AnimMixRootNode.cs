

using CustomPlayable;
using System.Collections.Generic;
using UnityEngine.Playables;

public class AnimMixRootNode : AnimMixNode
{
    public AnimMixRootNode(PlayableGraph graph, playableAnimator animator) : base(graph, animator)
    {
        Init();
        if(GameContext.GameMode == GameMode.DEBUG)
        {
            DebugManager.Instance?.AddMonitor(() => { return "MixRoot CurIndex = " + curIndex; });
            DebugManager.Instance?.AddMonitor(() => { return "MixRoot TargetIndex = " + targetIndex; });
            DebugManager.Instance?.AddMonitor(() => { return "MixRoot translateTime = " + translateTime; });
        }
    }

    /// <summary>
    /// 初始化配置的动画
    /// </summary>
    public void Init()
    {
/*        List<AnimInfoData> animInfoDatas = rootAnimtor.character.characterData.GetClipAnimInfos();
        foreach (var data in animInfoDatas)
        {
            if (data.animName == "") return;
            if (ExistAnim(data.animName)) return;
            var clip = ResourceManager.GetInstance().GetAnimatinClip(data.animName);
            AnimClipNode able = new AnimClipNode(graph, rootAnimtor, clip);
            var index = AddInput(able.GetPlayable());
            datas.Add(index, new AnimClipData(data.animName, able));
        }*/
    }
}