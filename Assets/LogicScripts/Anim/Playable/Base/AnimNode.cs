using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CustomPlayable
{
    public class AnimNode
    {
        public int Count = 0;

        public Playable root;

        public playableAnimator rootAnimtor;

        public PlayableGraph graph;

        public bool enable = false;

        public AnimNode() { }

        public AnimNode(PlayableGraph graph, playableAnimator animator)
        {
            this.graph = graph;
            root = ScriptPlayable<AnimBehaviour>.Create(graph);
            ((ScriptPlayable<AnimBehaviour>)root).GetBehaviour().Init(this);
            rootAnimtor = animator;
        }

        public virtual void Enable()
        {
            enable = true;
            //root.SetInputWeight(0, 1);
            //root.SetTime(0);
            root.Play();
        }

        public virtual void Disable()
        {
            enable = false;
            //root.SetInputWeight(0, 0);
            //root.SetTime(0);
            root.Pause();
        }

        public virtual void InitWeight()
        {
            
        }

        public void Clear()
        {
            root.Destroy();
            root = ScriptPlayable<AnimBehaviour>.Create(graph);
            ((ScriptPlayable<AnimBehaviour>)root).GetBehaviour().Init(this);
        }

        public virtual void Execute(Playable playable, FrameData info){}

        public virtual int AddInput(Playable able)
        {
            return AddRootInput(able);
        }

        public virtual int AddRootInput(Playable playable)
        {
            root.AddInput(playable, 0);
            root.SetInputWeight(0, 1);
            return root.GetInputCount();
        }

        public virtual Playable GetPlayable()
        {
            return root;
        }
    }
}