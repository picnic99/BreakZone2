﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AnimHelper
{
    public static void Enable(Playable playable)
    {
        var adapter = GetAdapter(playable);
        if (adapter!= null) adapter.Enable();
    }

    public static void Enable(AnimationMixerPlayable mixer,int index)
    {
        Enable(mixer.GetInput(index));
    }

    public static void Disable(Playable playable)
    {
        var adapter = GetAdapter(playable);
        if (adapter != null) adapter.Disable();
    }

    public static void Disable(AnimationMixerPlayable mixer, int index)
    {
        Disable(mixer.GetInput(index));
    }

    public static AnimAdapter GetAdapter(Playable playable)
    {
        if (typeof(AnimAdapter).IsAssignableFrom(playable.GetPlayableType()))
        {
            return (( ScriptPlayable<AnimAdapter>)playable).GetBehaviour();
        }
        return null;
    }

    public static void SetOutput(PlayableGraph graph,Animator animtor,AnimBehaviour behaviour)
    {
        var root = new Root(graph);
        root.AddInput(behaviour);
        AnimationPlayableOutput.Create(graph, "Anim", animtor).SetSourcePlayable(root.GetAnimAdapterPlayable());
    }

    public static void Start(PlayableGraph graph)
    {
        graph.Play();
        GetAdapter(graph.GetOutputByType<AnimationPlayableOutput>(0).GetSourcePlayable()).Enable();
    }

    public static ComputeShader GetCompute(string name)
    {
        return Object.Instantiate(Resources.Load<ComputeShader>("Compute/" + name));

    }
}
