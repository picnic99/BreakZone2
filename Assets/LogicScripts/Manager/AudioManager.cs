
using UnityEngine;

public class AudioManager : Singleton<AudioManager>, Manager
{

    private AudioSource loopAudio { get { return GameContext.GetLoopAudio(); } }
    private AudioSource onceAudio{ get { return GameContext.GetOnceAudio(); } }
    public void Init()
    {

    }

    public void Play(string clipName,bool isLoop)
    {
        AudioClip clip = ResourceManager.GetInstance().GetAudioClip(clipName);
        if (clip == null) return;
        if (isLoop)
        {
            onceAudio.PlayOneShot(clip);
        }
        else
        {
            onceAudio.PlayOneShot(clip);
        }
    }
}
