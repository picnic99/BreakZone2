using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class yuan : MonoBehaviour
{
    public float Radius = 6;    //�뾶  
    public int Segments = 60;   //�ָ���  
    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateMesh(Radius, Segments);
    }

    Mesh CreateMesh(float radius, int segments)
    {
        //vertices:
        int vertices_count = Segments + 1;
        Vector3[] vertices = new Vector3[vertices_count];
        vertices[0] = Vector3.zero;
        float angledegree = 360.0f;
        float angleRad = Mathf.Deg2Rad * angledegree;
        float angleCur = angleRad;
        float angledelta = angleRad / Segments;
        for (int i = 1; i < vertices_count; i++)
        {
            float cosA = Mathf.Cos(angleCur);
            float sinA = Mathf.Sin(angleCur);

            vertices[i] = new Vector3(Radius * cosA, 0, Radius * sinA);
            angleCur -= angledelta;
        }

        //triangles
        int triangle_count = segments * 3;
        int[] triangles = new int[triangle_count];
        for (int i = 0, vi = 1; i <= triangle_count - 1; i += 3, vi++)     //��Ϊ�ð����ָ���60�������Σ������һ������˳��Ӧ���ǣ�0 60 1��������Ҫ��������
        {
            triangles[i] = 0;
            triangles[i + 1] = vi;
            triangles[i + 2] = vi + 1;
        }
        triangles[triangle_count - 3] = 0;
        triangles[triangle_count - 2] = vertices_count - 1;
        triangles[triangle_count - 1] = 1;                  //Ϊ����ɱջ��������һ�������ε��������

        //uv:
        Vector2[] uvs = new Vector2[vertices_count];
        for (int i = 0; i < vertices_count; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
        }

        //����������mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        return mesh;
    }
}