using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DrawRangeManager : MonoBehaviour
{
    public Material lineMaterial; // 用于矩形的材质

    public Vector3 LT = Vector3.zero;
    public Vector3 RT = Vector3.zero;
    public Vector3 LB = Vector3.zero;
    public Vector3 RB = Vector3.zero;

    public int type = 1;

    private Color triggerColor;
    private Color missColor;
    private Color posColor;
    private Color color;

    private float alpha = 0.5f;

    public bool IsDraw = false;

    private void Awake()
    {
        triggerColor = new Color(0, 1, 0, alpha);

        missColor = new Color(1, 0, 0, alpha);

        posColor = new Color(186f/255f, 1, 1, alpha);

        color = triggerColor;

        if (!lineMaterial)
        {
            //Unity内置着色器，用于绘制简单色彩
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            //Shader shader = Shader.Find("Unlit/Color");
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

    public void SetRange(Vector3 LT, Vector3 RT, Vector3 LB, Vector3 RB)
    {
        IsDraw = true;
        this.LT = LT;
        this.RT = RT;
        this.LB = LB;
        this.RB = RB;
    }

    private void OnRenderObject()
    {
        if (!IsDraw) return;

        if(type == 1)
        {
            if (alpha >= 0.4f)
            {
                color = triggerColor;
            }
            else
            {
                color = missColor;
            }
            color.a = alpha;
            if (color.a <= 0)
            {
                MonoBridge.GetInstance().DestroyOBJ(gameObject);
            }
            alpha -= 0.5f * Time.deltaTime;
        }else if (type == 2)
        {
            color = posColor;
            color.a = alpha;
            if (color.a <= 0.1f)
            {
                MonoBridge.GetInstance().DestroyOBJ(gameObject);
            }
            alpha -= 0.05f * Time.deltaTime;

        }
        // 保存当前的渲染状态  
        lineMaterial.SetPass(0);
        GL.PushMatrix();

        // 应用变换矩阵（这里假设矩形位于世界坐标原点，没有旋转和缩放）  
        GL.MultMatrix(transform.localToWorldMatrix);

        // 开始绘制矩形  
        GL.Begin(GL.QUADS); // 使用四边形来绘制矩形  
        GL.Color(color); // 设置矩形颜色（这通常会被材质覆盖）  
        // 定义矩形的四个顶点（按顺时针或逆时针顺序）  
        GL.Vertex3(LB.x, Mathf.Max(LB.y, 0.1f), LB.z); ; // 左下角  
        GL.Vertex3(RB.x, Mathf.Max(RB.y, 0.1f), RB.z); // 右下角  
        GL.Vertex3(RT.x, Mathf.Max(RT.y, 0.1f), RT.z); // 右上角  
        GL.Vertex3(LT.x, Mathf.Max(LT.y, 0.1f), LT.z); // 左上角  

        GL.End(); // 结束绘制  

        // 恢复之前的渲染状态  
        GL.PopMatrix();
    }
}
