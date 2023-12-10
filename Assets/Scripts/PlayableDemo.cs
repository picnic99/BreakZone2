using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableDemo : MonoBehaviour
{
    public Animator anim;
    public AnimationClip clip0;
    public AnimationClip clip1;
    PlayableGraph graph;

    AnimationMixerPlayable mixPlayable;
    [Range(0,1)]
    public float weight;
    // Start is called before the first frame update
    void Start()
    {

        #region play a simple anim
        /*        graph = PlayableGraph.Create();
                graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
                var output = AnimationPlayableOutput.Create(graph,"anim1",anim);
                var clipPlayable = AnimationClipPlayable.Create(graph, clip);
                output.SetSourcePlayable(clipPlayable);
                graph.Play();*/


        //simple style
        //AnimationPlayableUtilities.PlayClip(anim, clip, out graph);
        #endregion

        graph = PlayableGraph.Create();
        var output = AnimationPlayableOutput.Create(graph, "anim2", anim);
        mixPlayable = AnimationMixerPlayable.Create(graph, 2);
        output.SetSourcePlayable(mixPlayable);

        var clipPlayable0 = AnimationClipPlayable.Create(graph, clip0);
        var clipPlayable1 = AnimationClipPlayable.Create(graph, clip1);

        graph.Connect(clipPlayable0, 0, mixPlayable, 0);
        graph.Connect(clipPlayable1, 0, mixPlayable, 1);

        graph.Play();

    }

    // Update is called once per frame
    void Update()
    {
        mixPlayable.SetInputWeight(0, weight);
        mixPlayable.SetInputWeight(1, 1-weight);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mixPlayable.SetTime(100f);
        }
    }

    private void OnDisable()
    {
        graph.Destroy();
    }
}
