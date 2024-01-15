
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>, Manager
{
    private AudioSourcePool pool;
    private AudioSource loopAudio { get { return GameContext.GetLoopAudio(); } }
    private AudioSource onceAudio { get { return GameContext.GetOnceAudio(); } }
    public void Init()
    {
        pool = new AudioSourcePool();
    }

    public void Play(string clipName, bool isLoop)
    {
        AudioClip clip = ResourceManager.GetInstance().GetAudioClip(clipName);
        if (clip == null) return;
        if (isLoop)
        {
            loopAudio.clip = clip;
            loopAudio.Play();
        }
        else
        {
            onceAudio.PlayOneShot(clip);
        }
    }

    public void Play(AudioVO audioVO, GameObject target = null)
    {
        if (audioVO != null)
        {
            if (audioVO.IsAllPlay())
            {
                //全部播放
                foreach (var item in audioVO.audio.AudioDatas)
                {
                    PlayAudio(item, target);
                }
            }
            else if (audioVO.IsRandomPlay())
            {
                //随机播放
                //TODO 使用权重变量 weight
                var index = UnityEngine.Random.Range(0, audioVO.audio.AudioDatas.Count);
                PlayAudio(audioVO.audio.AudioDatas[index], target);
            }
        }
    }

    public void PlayAudio(cfg.AudioData data, GameObject target)
    {
        AudioClip clip = ResourceManager.GetInstance().GetAudioClip(data.AudioName);
        if (clip == null) return;
        var audioSource = pool.Get();

        //设置音频到对应播放源
        //获取播放源

        if (target != null)
        {
            string sourceName = "";
            if (data.SourceType == cfg.AudioSourceType.CHARACTER_BODY)
            {
                sourceName = "bodyAudioSourceMain";
            }
            else if (data.SourceType == cfg.AudioSourceType.CHARACTER_MOUTH)
            {
                sourceName = "mouthAudioSourceMain";
            }
            else if (data.SourceType == cfg.AudioSourceType.SKILL)
            {
                sourceName = "skillAudioSourceMain";
            }

            var sourceMain = target.transform.Find(sourceName);
            if (sourceMain == null)
            {
                var tempObj = new GameObject(sourceName);
                tempObj.transform.parent = target.transform;
                sourceMain = tempObj.transform;
            }

            if (data.PlayType == 1)
            {
                //覆盖
                for (int i = 0; i < sourceMain.childCount; i++)
                {
                    var item = sourceMain.GetChild(i);
                    AudioSource ado = item.GetComponent<AudioSource>();
                    pool.Recover(ado);
                    //MonoBridge.GetInstance().DestroyOBJ(item.gameObject);
                }
            }

            audioSource.transform.parent = sourceMain.transform;
        }

        audioSource.volume = data.Volume;
        audioSource.pitch = data.Speed;
        audioSource.loop = data.Loop;
        audioSource.clip = clip;
        //考虑结束条件
        //1 时间 2 挂载物体消失  单次播放的 播放完自动回收 循环播放的 1 时间 2 物体消失 3等待事件
        if (data.Delay > 0)
        {
            audioSource.PlayDelayed(data.Delay);
        }
        else
        {
            audioSource.Play();
        }

        Action<object[]> objDestroyCall = null;
        //默认 物体消失时回收
        if (target != null)
        {
            objDestroyCall = (object[] args) =>
             {
                 var obj = (UnityEngine.Object)args[0];
                 if (obj != null)
                 {
                     if (obj == target)
                     {
                         pool.Recover(audioSource);
                         EventDispatcher.GetInstance().Off(EventDispatcher.OBJ_DESTROY, objDestroyCall);
                     }
                 }

             };

            EventDispatcher.GetInstance().On(EventDispatcher.OBJ_DESTROY, objDestroyCall);
        }

        //注册移除时机
        if (!data.Loop)
        {
            //播放完消失
            float playTime = clip.length / audioSource.pitch;
            TimeManager.GetInstance().AddOnceTimer(this, playTime, () =>
            {
                pool.Recover(audioSource);
                EventDispatcher.GetInstance().Off(EventDispatcher.OBJ_DESTROY, objDestroyCall);
            });
        }
        else
        {
            //最大循环时长
            if (data.MaxLoopTime > 0)
            {
                TimeManager.GetInstance().AddOnceTimer(this, data.MaxLoopTime, () =>
                {
                    pool.Recover(audioSource);
                    EventDispatcher.GetInstance().Off(EventDispatcher.OBJ_DESTROY, objDestroyCall);
                });
            }
        }
    }

    public void Stop(bool isLoop)
    {
        if (isLoop)
        {
            loopAudio.Stop();
        }
        else
        {
            onceAudio.Stop();
        }
    }
}
