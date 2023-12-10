using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

/*[System.Serializable]
public struct BlendClip2D
{
    public AnimationClip clip;
    public Vector2 pos;
}*/


public class BlendTree2D : AnimBehaviour
{
    private struct DataPair
    {
        public float x;
        public float y;
        public float output;
    }

    private AnimationMixerPlayable m_mixer;
    private DataPair[] m_datas;

    private ComputeShader m_computeShader;
    private ComputeBuffer m_computeBuffer;

    private int m_kernel;
    private int m_clipCount;
    private Vector2 m_pointer;

    private int m_pointerX;
    private int m_pointerY;
    private float m_total;

    public BlendTree2D(PlayableGraph graph, BlendClip2D[] clips, float enterTime = 0f,float eps = 1e-5f) : base(graph)
    {
        m_datas = new DataPair[clips.Length];

        m_mixer = AnimationMixerPlayable.Create(graph);
        m_adapterPlayable.AddInput(m_mixer,0,1f);

        for (int i = 0; i < clips.Length; i++)
        {
            m_mixer.AddInput(AnimationClipPlayable.Create(graph,clips[i].clip),0);

            m_datas[i].x = clips[i].pos.x;
            m_datas[i].y = clips[i].pos.y;
        }

        m_computeShader = AnimHelper.GetCompute("BlendTree2D");
        m_computeBuffer = new ComputeBuffer(clips.Length, 12);
        m_kernel = m_computeShader.FindKernel("Compute");
        m_computeShader.SetBuffer(m_kernel,"dataBuffer",m_computeBuffer);
        m_computeShader.SetFloat("eps",eps);

        m_clipCount = clips.Length;
        m_pointerX = Shader.PropertyToID("pointerX");
        m_pointerY = Shader.PropertyToID("pointerY");

        m_pointer.Set(1, 1);
        SetPointer(0, 0);

    }

    public override void Enable()
    {
        base.Enable();
        m_adapterPlayable.SetTime(0f);
        m_mixer.SetTime(0f);
        m_adapterPlayable.Play();
        m_mixer.Play();

        for (int i = 0; i < m_clipCount; i++)
        {
            m_mixer.GetInput(i).SetTime(0);
            m_mixer.GetInput(i).Play();
        }

        SetPointer(0,0);
    }

    public override void Disable()
    {
        base.Disable();

        m_adapterPlayable.Pause();
        m_mixer.Pause();

        for (int i = 0; i < m_clipCount; i++)
        {
            m_mixer.GetInput(i).Pause();
        }
    }

    public void SetPointer(Vector2 vector2)
    {
        SetPointer(vector2.x, vector2.y);
    }


    public void SetPointer(float x,float y)
    {
        if(m_pointer.x == x && m_pointer.y == y)
        {
            return;
        }

        m_pointer.Set(x, y);

        m_computeShader.SetFloat(m_pointerX,x);
        m_computeShader.SetFloat(m_pointerY,y);
        m_computeBuffer.SetData(m_datas);
        m_computeShader.Dispatch(m_kernel,m_clipCount,1,1);
        m_computeBuffer.GetData(m_datas);

        int i;
        for (i = 0; i < m_clipCount; i++)
        {
            m_total += m_datas[i].output;
        }

        for (i = 0; i < m_clipCount; i++)
        {
            m_mixer.SetInputWeight(i,m_datas[i].output/m_total);
        }

        m_total = 0f;

    }

    public override void Stop()
    {
        base.Stop();
        m_computeBuffer.Dispose();
    }
}
