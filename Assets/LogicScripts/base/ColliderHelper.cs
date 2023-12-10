using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillColliderInfo
{
    public LayerMask layer;
    public CharacterState TriggerType;
}

public class ColliderHelper : MonoBehaviour
{
    public SkillColliderInfo info;

    public Action<Collider> OnTriggerEnterCall;
    public Action<Collider> OnTriggerStayCall;
    public Action<Collider> OnTriggerExitCall;
    public Action<Collision> OnCollisionEnterCall;
    public Action<Collision> OnCollisionExitCall;
    public Action<Collision> OnCollisionStayCall;

    /// <summary>
    /// 设置检测信息
    /// </summary>
    /// <param name="layer">检测的层级 角色层、建筑层 后续可能做一个可以与地形互动</param>
    /// <param name="TriggerType">检测的类型 1、好友 2、敌人 </param>
    public void SetInfo(LayerMask layer, CharacterState TriggerType)
    {
        info = new SkillColliderInfo();
        info.layer = layer;
        info.TriggerType = TriggerType;
    }

    public void SetInfo()
    {
        info = new SkillColliderInfo();
        info.layer = LayerMask.NameToLayer("Character");
        info.TriggerType = CharacterState.ENEMY;
    }

    public SkillColliderInfo GetInfo()
    {
        return info;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterCall?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayCall?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitCall?.Invoke(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterCall?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExitCall?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionStayCall?.Invoke(collision);
    }


    // Update is called once per frame
    void OnRenderObject()
    {
        var colliders = gameObject.GetComponents<BoxCollider>();
        if (colliders == null)
        {
            return;
        }
        CreateLineMaterial();
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        for (int i = 0; i < colliders.Length; i++)
        {
            var col = colliders[i];
            var c = col.center;
            var size = col.size;
            float rx = size.x / 2f;
            float ry = size.y / 2f;
            float rz = size.z / 2f;
            Vector3 p0, p1, p2, p3;
            Vector3 p4, p5, p6, p7;
            p0 = c + new Vector3(-rx, -ry, rz);
            p1 = c + new Vector3(rx, -ry, rz);
            p2 = c + new Vector3(rx, -ry, -rz);
            p3 = c + new Vector3(-rx, -ry, -rz);

            p4 = c + new Vector3(-rx, ry, rz);
            p5 = c + new Vector3(rx, ry, rz);
            p6 = c + new Vector3(rx, ry, -rz);
            p7 = c + new Vector3(-rx, ry, -rz);

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p0);
            GL.Vertex(p1);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p2);
            GL.Vertex(p3);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p0);
            GL.Vertex(p3);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p4);
            GL.Vertex(p5);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p5);
            GL.Vertex(p6);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p6);
            GL.Vertex(p7);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p4);
            GL.Vertex(p7);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p0);
            GL.Vertex(p4);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p1);
            GL.Vertex(p5);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p2);
            GL.Vertex(p6);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(p3);
            GL.Vertex(p7);
            GL.End();
        }
        GL.PopMatrix();
    }

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
}

