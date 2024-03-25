using cfg;
using System;
using UnityEngine;

public class AudioEventDispatcher: Singleton<AudioEventDispatcher>
{
    public _EventDispatcher dispatcher;

    public void On(string eventName, Action<object[]> callback)
    {
        dispatcher.On(eventName, callback);
    }

    public void Off(string eventName, Action<object[]> callback)
    {
        dispatcher.Off(eventName, callback);
    }

    public void Event(string eventName, params object[] args)
    {
        dispatcher.Event(eventName, args);
    }

    public void Init()
    {
        dispatcher = new _EventDispatcher();
        dispatcher.On(MomentType.DoSkill, Handle_DoSkill);
        dispatcher.On(MomentType.EnterState, Handle_EnterState);
    }

    private void Handle_DoSkill(object[] args)
    {
        Skill skill = (Skill)args[0];
        string keyword = (string)args[1];
        
        AudioVO vo = AudioConfiger.GetInstance().GetAudioData(0, skill.skillData.Id, 0, keyword);
        if (vo!= null)
        {
            GameObject target = null;
            if (args.Length >= 3)
            {
                target = (GameObject)args[2];
            }
            AudioManager.GetInstance().Play(vo, target);
        }
        else
        {
            Debug.LogError("AudioConfiger配置有误！！！");
        }
    }

    private void Handle_EnterState(object[] args)
    {
        int crtId = (int)args[0];
        int stateId = (int)args[1];
        string keyword = (string)args[2];
        AudioVO vo = AudioConfiger.GetInstance().GetAudioData(crtId, 0, stateId, keyword);
        if (vo != null)
        {
            GameObject target = null;
            if (args.Length >= 4)
            {
                target = (GameObject)args[3];
            }
            AudioManager.GetInstance().Play(vo, target);
        }
        else
        {
            Debug.LogError($"AudioConfiger配置有误！！！: {crtId}_0_{stateId}_{keyword}");
        }
    }
}