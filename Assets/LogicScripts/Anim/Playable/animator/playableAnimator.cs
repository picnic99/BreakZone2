using CustomPlayable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class playableAnimator
{
    /// <summary>
    /// 分发节点
    /// </summary>
    protected AnimDispatcherNode rootNode;

    protected PlayableGraph graph;
    private AnimationPlayableOutput output;
    private Animator _anim;
    //public Character character { get; set; }

    /// <summary>
    /// 状态机的速度
    /// </summary>
    private float _speed = 1;
    public float Speed { get { return _speed; } }
    /// <summary>
    /// 初始化动画状态机
    /// </summary>
    public virtual void Init(Animator anim)
    {
        _anim = anim;
        //this.character = character;

        graph = PlayableGraph.Create();
        output = AnimationPlayableOutput.Create(graph, this.GetType().Name, _anim);
        //连接分发节点 分发节点根据动画数据初始化动画树
        rootNode = new AnimDispatcherNode(graph, this);

        output.SetSourcePlayable(rootNode.GetPlayable());
        graph.Play();
    }

    public virtual void Play(string animName, float time, bool isPercent = true)
    {
        rootNode.PlayAnim(animName, time, isPercent);
    }

    public virtual void Stop(string animName, float time)
    {
        rootNode.StopAnim(animName, time);
    }

    public virtual void SetSpeed(float value)
    {
        _speed = value;
    }
}
