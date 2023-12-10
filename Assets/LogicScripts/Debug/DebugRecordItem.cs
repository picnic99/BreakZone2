using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugRecordItem
{
    public GameObject Root;
    public string recordMsg;

    private Text msgText;

    public DebugRecordItem(GameObject root)
    {
        this.Root = root;
        msgText = Root.GetComponent<Text>();
    }

    public void UpdateMsg(string msg)
    {
        recordMsg = msg;
        msgText.text = "[" + GetTime() + "]" + recordMsg;
    }

    private string GetTime()
    {
        var hour = DateTime.Now.Hour;
        var minute = DateTime.Now.Minute;
        var second = DateTime.Now.Second;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
    }
}
