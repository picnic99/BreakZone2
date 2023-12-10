using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DebugSkillEditor : MonoBehaviour
{
    public Animator anim;

    public Slider slider;

    public GameObject effect;

    public float lastFrame = 0f;

    private string animName = "Hard_Attk_4";
    private AnimationClip curClip;

    private Text infoTxt;
    private GameObject tmp_frame;
    private Dropdown frameRateOption;
    private GameObject curFrame;


    private float totalFrameCtn;
    private float totalSeconds;

    private float defaultWidth;

    private List<GameObject> timeFrames = new List<GameObject>();

    private void Init()
    {
        infoTxt = transform.Find("TimeLine/TimeAreaOpt/infoTxt").GetComponent<Text>();
        frameRateOption = transform.Find("TimeLine/TimeAreaOpt/frameSize").GetComponent<Dropdown>();
        tmp_frame = transform.Find("TimeLine/Scroll View/Viewport/Content/Tmp_Frame").gameObject;
        curFrame = transform.Find("TimeLine/Scroll View/Viewport/Content/curFrame").gameObject;
        tmp_frame.SetActive(false);
        defaultWidth = tmp_frame.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void Start()
    {
        Init();

        anim.speed = 1;
        //anim.SetTrigger("atk1");
        var clips = anim.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips[i];
            if (clip.name == animName)
            {
                curClip = clip;
                break;
            }
        }
        totalFrameCtn = curClip.frameRate * curClip.length;
        totalSeconds = curClip.length;
        UpdateTimeView();
        frameRateOption.onValueChanged.AddListener(OnRateOptionChange);
    }

    private void OnRateOptionChange(int index)
    {
        UpdateTimeView();
    }

    private void UpdateTimeView()
    {
        //当前选择
        var rate = Convert.ToInt32( frameRateOption.options[frameRateOption.value].text);
        var num = Mathf.Ceil(totalFrameCtn / rate);
        HideItems();
        for (int i = 0; i < num; i++)
        {
            GameObject item = null;
            if(i > timeFrames.Count - 1)
            {
                item = Instantiate(tmp_frame, tmp_frame.transform.parent);
                timeFrames.Add(item);
            }
            else
            {
                item = timeFrames[i];
            }
            var rect = item.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(defaultWidth, rect.sizeDelta.y);
            var txt = item.transform.Find("timeLine/frameTxt").GetComponent<Text>();
            txt.text = (i + 1) * rate + "";
            if (i == num - 1)
            {
                var n = totalFrameCtn % rate;
                if(n > 0)
                {
                    rect.sizeDelta = new Vector2(defaultWidth / rate * n, rect.sizeDelta.y);
                    txt.text = totalFrameCtn + "";
                }
            }
            item.SetActive(true);
        }
        curFrame.transform.SetAsLastSibling();
    }

    private void HideItems()
    {
        foreach (var item in timeFrames)
        {
            item.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        
        var curFrameCtn = (curClip.frameRate * curClip.length) * slider.value;
        var curSeconds = curFrameCtn / curClip.frameRate;
        var rate = curFrameCtn / totalFrameCtn * 100;
        curClip.SampleAnimation(anim.gameObject, curSeconds);
        infoTxt.text = $"frame:{curFrameCtn.ToString("F0")}/{totalFrameCtn.ToString("F0")} second:{curSeconds.ToString("F2")}/{totalSeconds.ToString("F2")}s rate:{rate.ToString("F2")}%";
        var effectTime = ParticleSystemLength(effect.transform);
        var t = effectTime * slider.value;
        PlayEffectByTime(effect, t);
        //anim.Play("普通攻击一段", 0, slider.value);
    }


    public void PlayEffectByTime(GameObject eff, float time)
    {
        var pts = eff.GetComponentsInChildren<ParticleSystem>();
        foreach (var e in pts)
        {
            e.Simulate(time);
        }
    }


    public float ParticleSystemLength(Transform transform)
    {
        var pts = transform.GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0f;
        foreach (var p in pts)
        {
            if (p.enableEmission)
            {
                if (p.loop)
                {
                    return -1f;
                }
                float dunration = 0f;
                if (p.emissionRate <= 0)
                {
                    dunration = p.startDelay + p.startLifetime;
                }
                else
                {
                    dunration = p.startDelay + Mathf.Max(p.duration, p.startLifetime);
                }
                if (dunration > maxDuration)
                {
                    maxDuration = dunration;
                }
            }
        }
        return maxDuration;
    }
}
