using UnityEngine;

public class AudioSourcePool : ObjPool<AudioSource>
{
    public override void Init(AudioSource obj)
    {
        if (obj == null) return;
        obj.pitch = 1;
        obj.clip = null;
        obj.volume = 1;
        obj.loop = false;
        obj.maxDistance = 5;
        obj.transform.parent = GameContext.GetAudiosMain().transform;
    }

    public override AudioSource InstanceObj()
    {
        GameObject gameObject = new GameObject("Pool_Audio");
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        Init(audioSource);
        return audioSource;
    }
}