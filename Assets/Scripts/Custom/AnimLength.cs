using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLength : MonoBehaviour
{
    public AnimationClip clip;

    public float len;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(clip != null)
        {
            len = clip.length;
        }
    }
}
