using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Action<object[]> action;
    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.GetInstance().On("1", A);

        /*        action += A;
                action += B;
                action += C;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //action.Invoke(new object[] { });
            Debug.Log("111");
            EventDispatcher.GetInstance().Event("1", new object[] { });
            Debug.Log("222");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //action -= A;
            //action -= B;
            EventDispatcher.GetInstance().Off("1", B);
            EventDispatcher.GetInstance().Off("1", A);
        }
    }



    public void A(object[] args)
    {
        Debug.Log("A");
    }

    public void B(object[] args)
    {
        Debug.Log("B");
    }

    public void C(object[] args)
    {
        Debug.Log("C");
    }
}
