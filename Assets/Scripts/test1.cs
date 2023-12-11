using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    void Start()
    {
        //EventDispatcher.GetInstance().On("1", B);
        /*        action += A;
                action += B;
                action += C;*/

        TEST("-45,45","0,-120",1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TEST(string degreeH,string degreeV,float radius)
    {
/*        string[] s_degH = degreeH.Split(","); string[] s_degV = degreeV.Split(",");
        float[] f_degH = new float[2]; float[] f_degV = new float[2];
        f_degH[0] = Convert.ToSingle(s_degH[0]) * Mathf.Deg2Rad ; f_degV[0] = Convert.ToSingle(s_degV[0]) * Mathf.Deg2Rad;
        f_degH[1] = Convert.ToSingle(s_degH[1]) * Mathf.Deg2Rad; f_degV[1] = Convert.ToSingle(s_degV[1]) * Mathf.Deg2Rad;
        //À„≥ˆ∂•µ„”Ú
        float[] areaX = new float[] { Mathf.Sin(f_degH[0]) * radius, Mathf.Sin(f_degH[1]) * radius };
        float[] areaY = new float[] { Mathf.Sin(f_degV[0]) * radius, Mathf.Sin(f_degV[1]) * radius };
        float[] areaZ = new float[] { Mathf.Sin(f_degV[0]) * radius, Mathf.Sin(f_degV[1]) * radius };

        Debug.Log($"X [{areaX[0]},{areaX[1]}]\nY [{areaY[0]},{areaY[1]}]\nZ [{areaZ[0]},{areaZ[1]}]\n");*/
    }

    public void B(object[] args)
    {
        Debug.Log("AABBQQ");
    }
}
