using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float t = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime;
        if (t > 0)
        {
            transform.position += transform.forward * Time.deltaTime * 20f;
            transform.localScale += Vector3.one * Time.deltaTime * 2f;
        }
        else
        {
            transform.position -= transform.forward * Time.deltaTime * 20f;
            transform.localScale -= Vector3.one * Time.deltaTime * 2f;


        }
    }
}
