using CustomPlayable;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 角色 动画分发节点
/// 作用 根据传递的动画名称
/// 查询动画属于哪个节点 然后再调用对应节点的动画
/// </summary>
public class AnimDispatcherNode : AnimMixNode
{
    /// <summary>
    /// clip类型动画总节点
    /// </summary>
    public AnimMixRootNode clipMixNode;
    /// <summary>
    /// blend类型动画总节点
    /// </summary>
    public AnimBlendRootNode blendMixNode;

    public int CLIP_INDEX = 0;
    public int BLEND_INDEX = 1;

    public AnimDispatcherNode(PlayableGraph graph, playableAnimator animator) : base(graph, animator)
    {
        clipMixNode = new AnimMixRootNode(graph, animator);
        blendMixNode = new AnimBlendRootNode(graph, animator);
        AddInput(clipMixNode.GetPlayable());
        AddInput(blendMixNode.GetPlayable());
        datas.Add(CLIP_INDEX, new AnimClipData("clipMixNode", clipMixNode));
        datas.Add(BLEND_INDEX, new AnimClipData("blendMixNode", blendMixNode));
        Init();
        Enable();
    }

    public void Init()
    {

    }

    public override void TranslateTo(string animKey, float time, bool isPercent = true)
    {

        Debug.Log("Dispatcher:" + animKey);
        var animInfo = AnimConfiger.GetInstance().GetAnimByAnimKey(animKey); //RegAnimKey.GetKeys(targetAnimName);
        if (animInfo == null) return;
        if (animInfo.animation.AnimType == AnimType.CLIP)
        {
            TranslateIndex(CLIP_INDEX, 0.15f);
            //blendMixNode.ClearAllWeight();
            clipMixNode.TranslateTo(animInfo.animation.Clips[0].AnimPath, time, isPercent);
        }
        else if (animInfo.animation.AnimType == AnimType.BLEND)
        {
            TranslateIndex(BLEND_INDEX, 0.15f);
            //clipMixNode.ClearAllWeight();
            blendMixNode.TranslateTo(animInfo.animation.Key, 0.3f, isPercent);
        }
    }


    private void TranslateIndex(int tarIndex, float time, bool isBlend = false)
    {
        if (curIndex == tarIndex && !isTranlating) return;

        //若当前无播放的动画 则直接播放该动画 权重设为1
        if (curIndex == -1)
        {
            curIndex = tarIndex;
            GetAnimClipData(curIndex).able.Enable();
            SetWeight(curIndex, 1);
            return;
        }

        if (isTranlating)
        {
            if (isBlend)
            {
                if (tarIndex == targetIndex) return;

                if (GetWeight(curIndex) > GetWeight(targetIndex))
                {
                    dels.Add(targetIndex);
                }
                else
                {
                    dels.Add(curIndex);
                }
                curIndex = targetIndex;
            }
            else
            {
                if (curIndex == tarIndex)
                {
                    curIndex = targetIndex;
                }
            }

        }

        targetIndex = tarIndex;
        GetAnimClipData(targetIndex).able.Enable();

        if (dels.IndexOf(targetIndex) != -1)
        {
            dels.Remove(targetIndex);
        }

        translateTime = time;
        curSpeed = GetWeight(curIndex) / time;
        delSpeed = 2 * curSpeed;

        isTranlating = true;
    }

}