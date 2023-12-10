using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using CustomPlayable;

public class PlayableTest : MonoBehaviour
{
    PlayableGraph graph; 
    public Animator anim;

    public AnimationClip clip1;

    private AnimClipNode clipNode1;
    private AnimMixNode mixer;

    void Start()
    {
        graph = PlayableGraph.Create();
        //clipNode1 = new AnimClipNode(graph, clip1);
        var output = AnimationPlayableOutput.Create(graph, "playable", anim);

/*        mixer = new AnimMixNode(graph);

        output.SetSourcePlayable(mixer.GetPlayable());*/
        graph.Play();
    }


    public int index = 1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //clipNode1.Enable();
            mixer.TranslateTo("idle", 0.2f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            //clipNode1.Disable();
            index++;
            if (index > 3) index = 1;
            mixer.TranslateTo("atk" + index, 0.2f);
        }
    }

    private void OnDestroy()
    {
        
    }
}
