using CustomPlayable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class playableAnimator
{
    /// <summary>
    /// �ַ��ڵ�
    /// </summary>
    protected AnimDispatcherNode rootNode;

    protected PlayableGraph graph;
    private AnimationPlayableOutput output;
    private Animator _anim;
    public Assets.LogicScripts.Client.Entity.Character Crt;
    //public Character character { get; set; }

    /// <summary>
    /// ״̬�����ٶ�
    /// </summary>
    private float _speed = 1;
    public float Speed { get { return _speed; } }
    /// <summary>
    /// ��ʼ������״̬��
    /// </summary>
    public virtual void Init(Animator anim, Assets.LogicScripts.Client.Entity.Character crt)
    {
        _anim = anim;
        //this.character = character;

        graph = PlayableGraph.Create();
        output = AnimationPlayableOutput.Create(graph, this.GetType().Name, _anim);
        //���ӷַ��ڵ� �ַ��ڵ���ݶ������ݳ�ʼ��������
        rootNode = new AnimDispatcherNode(graph, this);
        Crt = crt;
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

    public void OnDestroy()
    {
        graph.Destroy();
        _anim = null;
        rootNode = null;
    }
}
