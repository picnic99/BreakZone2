using CustomPlayable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugAnimItem
{
    public GameObject tmp_anim;
    public Text content;

    public AnimMixNode node;

    public DebugAnimItem(GameObject tmp_anim, AnimMixNode node)
    {
        this.tmp_anim = tmp_anim;
        this.node = node;
        content = tmp_anim.transform.Find("Text").GetComponent<Text>();
    }

    public void updateData()
    {
        // nodeName£ºblendRoot
        //transitionState£ºtrue
        // curIndex=0,W=0.2 targetIndex=1,W=0.3
        // dels[{ 2,0.5}]
        // addSpeed: 0.01  delSpeed: 0.02

        if (node == null) return;

        string name = node.GetType().Name;
        string transitionState = "transitionState=" + node.isTranlating.ToString();
        string indexMsg = "cur="+node.curIndex+" W=" + node.GetWeight(node.curIndex).ToString("F2") + " tar="+node.targetIndex + " W=" + node.GetWeight(node.targetIndex).ToString("F2");
        string delsMsg = node.dels.Count +"[";
        foreach (var del in node.dels)
        {
            delsMsg += "{I=" + del + " W=" + node.GetWeight(del).ToString("F2") + "}";
        }
        delsMsg += "]";
        string speedMsg = "addSpeed="+node.curSpeed.ToString("F2") + "delSpeed="+node.delSpeed.ToString("F2");

        SetContent(name + "\n" + transitionState + "\n" + indexMsg + "\n" + speedMsg + "\n" + delsMsg);
    }


    public void SetContent(string str)
    {
        content.text = str;
    }

    public void OnDestory()
    {
        GameObject.Destroy(tmp_anim);
        node = null;
    }
}
