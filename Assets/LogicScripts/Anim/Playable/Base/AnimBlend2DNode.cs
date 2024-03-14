using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CustomPlayable
{
    public class AnimBlend2DNode : AnimNode
    {
        private struct DataPair
        {
            public float x;
            public float y;
            public float output;
        }

        private AnimationMixerPlayable mixer;
        private DataPair[] datas;
        private ComputeShader computeShader;
        private ComputeBuffer computeBuffer;
        private int kernel;
        private int clipCount;
        private float eps = 1e-5f;

        private Vector2 pointer;
        private int pointerX;
        private int pointerY;

        private float total;

        private string AnimKey;

        private List<AnimClipNode> nodes = new List<AnimClipNode>();

        //欲监视的值
        private Vector2 monitorValue;

        public AnimBlend2DNode(PlayableGraph graph, string animKey, playableAnimator animator) : base(graph, animator)
        {
            mixer = AnimationMixerPlayable.Create(graph);
            AnimKey = animKey;
            AddRootInput(mixer);
        }

        public override void Enable()
        {
            foreach (var item in nodes)
            {
                item.Enable();
            }
            base.Enable();
        }

        public override void Disable()
        {
            foreach (var item in nodes)
            {
                item.Disable();
            }
            base.Disable();
        }

        public void Init()
        {
            var animInfo = AnimConfiger.GetInstance().GetAnimByAnimKey(AnimKey);// RegAnimKey.GetKeys(AnimKey);
            List<cfg.AnimClipData> clips = animInfo.animation.Clips;
            datas = new DataPair[clips.Count];
            for (int i = 0; i < clips.Count; i++)
            {
                var animClip = ResourceManager.GetInstance().GetAnimatinClip(clips[i].AnimPath);
                var clip = new AnimClipNode(graph, rootAnimtor, animClip);
                nodes.Add(clip);
                AddInput(clip.GetPlayable());
                clip.Enable();
                datas[i].x = clips[i].X;
                datas[i].y = clips[i].Y;
            }

            computeShader = ResourceManager.GetInstance().GetObjectInstance<ComputeShader>("Shader/BlendTree2D");
            computeBuffer = new ComputeBuffer(clips.Count, 12);
            kernel = computeShader.FindKernel("Compute");
            computeShader.SetBuffer(kernel, "dataBuffer", computeBuffer);
            computeShader.SetFloat("eps", eps);
            clipCount = clips.Count;
            pointerX = Shader.PropertyToID("pointerX");
            pointerY = Shader.PropertyToID("pointerY");
        }

        public override void Execute(Playable playable, FrameData info)
        {
            if (enable == false) return;

            var c = rootAnimtor.Crt;
            var x = c.CrtData.Input.x;
            var y = c.CrtData.Input.y;

            CommonUtils.Logout("playerId = " + c.playerId + "input = " + c.CrtData.Input.ToString());

            if (pointer.x == x && pointer.y == y)
            {
                return;
            }

            pointer.Set(x, y);

            computeShader.SetFloat(pointerX, x);
            computeShader.SetFloat(pointerY, y);
            computeBuffer.SetData(datas);
            computeShader.Dispatch(kernel, clipCount, 1, 1);
            computeBuffer.GetData(datas);

            for (int i = 0; i < clipCount; i++)
            {
                total += datas[i].output;
            }
            for (int i = 0; i < clipCount; i++)
            {
                SetWeight(i, datas[i].output / total);
            }
            total = 0f;
        }

        public override int AddInput(Playable able)
        {
            mixer.AddInput(able, 0);
            return mixer.GetInputCount() - 1;
        }

        public void SetWeight(int index, float w)
        {
            w = Mathf.Clamp(w, 0, 1);
            mixer.SetInputWeight(index, w);
        }

        public float GetWeight(int index)
        {
            return mixer.GetInputWeight(index);
        }
    }
}