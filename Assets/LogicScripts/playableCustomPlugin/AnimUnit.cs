using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AnimUnit : AnimBehaviour
{
    private AnimationClipPlayable m_anim;
    public AnimUnit(PlayableGraph graph,AnimationClip clip,float enterTime = 0f) : base(graph,enterTime)
    {
        m_anim = AnimationClipPlayable.Create(graph, clip);
        m_adapterPlayable.AddInput(m_anim, 0, 1f);

        Disable();
    }

    public override void Enable()
    {
        base.Enable();
        m_adapterPlayable.SetTime(0f);
        m_anim.SetTime(0f);
        m_adapterPlayable.Play();
        m_anim.Play();
    }

    public override void Disable()
    {
        base.Disable();
        m_adapterPlayable.Pause();
        m_anim.Pause();
    }
}
