using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimAdapter : PlayableBehaviour
{
    private AnimBehaviour m_behaviour;

    public void Init(AnimBehaviour behaviour)
    {
        m_behaviour = behaviour;
    }


    public void Enable()
    {
        m_behaviour?.Enable();
    }

    public void Disable()
    {
        m_behaviour?.Disable();
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        m_behaviour?.Execute(playable, info);
    }

    public float GetEnterTime()
    {
        return m_behaviour.enterTime;
    }

    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
        m_behaviour?.Stop();
    }
}
