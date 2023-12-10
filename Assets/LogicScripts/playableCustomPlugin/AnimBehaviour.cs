using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class AnimBehaviour
{
    public bool enable { get; protected set; }
    public float enterTime { get; protected set; }

    protected Playable m_adapterPlayable;

    public AnimBehaviour() { }

    public AnimBehaviour(PlayableGraph graph,float time = 0f)
    {
        m_adapterPlayable = ScriptPlayable<AnimAdapter>.Create(graph);
        ((ScriptPlayable<AnimAdapter>)m_adapterPlayable).GetBehaviour().Init(this);

        enterTime = time;
    }


    public virtual void Enable() {
        enable = true;
    }

    public virtual void Disable() {
        enable = false;
    }

    public virtual void Execute(Playable playable, FrameData info) {
        if (!enable) return;
    }

    public Playable GetAnimAdapterPlayable()
    {
        return m_adapterPlayable;
    }

    public virtual void AddInput(Playable playable) { }

    public void AddInput(AnimBehaviour anim)
    {
        AddInput(anim.GetAnimAdapterPlayable());
    }

    public virtual void Stop() { }

}
