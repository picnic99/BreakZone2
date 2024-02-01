using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDrawer : MonoBehaviour
{
    public static Color FRIEND_COLOR = new Color(0, 1, 0, 0.4f);
    public static Color ENEMY_COLOR = new Color(1, 0, 0, 0.4f);

    //当添加到一个物体上时，从物体位置出发，绘制带色彩的射线
    public Color color = FRIEND_COLOR;
    public float degree = 70;
    public float radius = 2.5f;
    public float radiusInterval = 1F;
    private Transform m_transform;
    static Material lineMaterial;
    /// <summary>
    /// 三角形的后两个顶点
    /// </summary>
    private List<Vector3> vertexs;
    /// <summary>
    /// 射线的发射方向
    /// </summary>
    private List<Vector3> dirs;
    RaycastHit hit;
    private float startTime = 0F;
    public float intervalTime = 0.1F;

    private Matrix4x4 curMatrix;

    public bool isShow = false;
    static void CreateLineMaterial()
    {
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

    public void ShowRange(float degree, float radius)
    {
        this.degree = degree;
        this.radius = radius;

        m_transform = transform;
        curMatrix = transform.localToWorldMatrix;
        vertexs = new List<Vector3>();//三角形的后两个顶点
        dirs = new List<Vector3>();//数目为degree+1
        float angle = -degree / 2F;
        for (float i = 0; i <= degree; i += radiusInterval)
        {
            float currentAngle = angle + i;
            float rad1 = currentAngle * Mathf.Deg2Rad;
            dirs.Add(new Vector3(Mathf.Sin(rad1), 0.02f, Mathf.Cos(rad1)));


            //currentAngle = i;
            //rad = currentAngle * Mathf.Deg2Rad;
            //dirs.Add(new Vector3(0, Mathf.Sin(rad1) , Mathf.Cos(rad1)));
            //dirs.Add(new Vector3(0, Mathf.Cos(rad) , Mathf.Sin(rad)));s
            //dirs.Add(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad) , 0));

/*            for (float j = 0; j <= degree; j += radiusInterval)
            {
                currentAngle = j;
                float rad = currentAngle * Mathf.Deg2Rad;
                dirs.Add(new Vector3(Mathf.Sin(rad) * Mathf.Sin(rad1), Mathf.Cos(rad), Mathf.Sin(rad)* Mathf.Cos(rad1)));
            }*/
        }

        startTime = Random.Range(0, intervalTime);

        isShow = true;
    }

    public void HideRange()
    {
        isShow = false;
    }

    private void Awake()
    {
        CreateLineMaterial();
    }
    private void Update()
    {
        if (!isShow) return;

        startTime += Time.deltaTime;
        if (startTime > intervalTime)
        {
            vertexs.Clear();
            Vector3 pos = m_transform.position;
            foreach (var dir in dirs)
            {
                /*                Vector3 worldDir = m_transform.TransformDirection(dir);//将模型空间坐标转换为世界空间坐标
                                if (Physics.Raycast(pos, worldDir, out hit, radius))
                                {
                                    //将世界坐标转换为模型坐标后存到vertex中
                                    vertexs.Add(m_transform.InverseTransformPoint(hit.point));
                                }
                                else
                                {
                                    vertexs.Add(dir * radius);
                                }*/
                vertexs.Add(dir * radius);
            }
            startTime = 0F;
        }

    }
    // 当所有常规渲染完成后调用
    public void OnRenderObject()
    {

        if (!isShow) return;

        lineMaterial.SetPass(0);

        GL.PushMatrix();

        GL.MultMatrix(curMatrix);

        GL.Begin(GL.QUADS);

        if (vertexs.Count > 0)
        {

            for (int i = 0; i < dirs.Count - 1; i++)
            {
                GL.Color(color);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(vertexs[i].x, vertexs[i].y, vertexs[i].z);
                GL.Vertex3(vertexs[i + 1].x, vertexs[i + 1].y, vertexs[i + 1].z);
            }
        }

        GL.End();

        GL.PopMatrix();
    }
}

