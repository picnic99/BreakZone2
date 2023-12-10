using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 direct;
    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 speed = new Vector3(hor, 0, ver) * 1 * 0.05f;

        cc.Move(speed);
        //var v = new Vector3(hor, 0, ver);
        //if (v.magnitude != 0) direct = v.normalized;
        //transform.forward = direct;
    }
}