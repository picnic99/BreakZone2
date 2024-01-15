using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audios;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            audios = GetComponent<AudioSource>();
        }
    }

    public void Play()
    {
        audios.PlayScheduled(14);
    }
}
