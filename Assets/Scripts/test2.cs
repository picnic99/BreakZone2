using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{

    public bool inRange = false;
    public GameObject obj;
    [Range(-180,180)]
    public float angleL;
    [Range(-180, 180)]
    public float angleR;
    public Vector3 left;
    public Vector3 right;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (obj == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward.normalized);
        Gizmos.color = Color.blue;
        var dir1 = Quaternion.AngleAxis(angleL, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + dir1);
        Gizmos.color = Color.green;
        var dir2 = Quaternion.AngleAxis(angleR, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + dir2);
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, 1);

        var self2Target = (obj.transform.position - transform.position).normalized;

        var left_border = Quaternion.AngleAxis(angleL, Vector3.up) * transform.forward;
        var right_border = Quaternion.AngleAxis(angleR, Vector3.up) * transform.forward;

        var left_crs = Vector3.Cross(self2Target.normalized, left_border.normalized);
        var right_crs = Vector3.Cross(self2Target.normalized, right_border.normalized);

        angle = Vector3.SignedAngle(transform.forward, self2Target, Vector3.up);

        inRange = false;
        if (left.y * right.y <= 0 || Mathf.Abs(angleL)+Mathf.Abs(angleR)>=360)
        {
            Vector3 targetPos = transform.position; targetPos.y = 0;
            Vector3 characterPos = obj.transform.position; characterPos.y = 0;
            if (Vector3.Distance(targetPos, characterPos) <= 1)
            {
                inRange = true;
            }
        }



    }
}
