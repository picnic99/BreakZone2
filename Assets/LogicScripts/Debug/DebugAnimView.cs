using CustomPlayable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAnimView
{
    public GameObject tmp_anim;

    public List<DebugAnimItem> anims; 

    public DebugAnimView(GameObject tmp_anim)
    {
        this.tmp_anim = tmp_anim;
        anims = new List<DebugAnimItem>();
    }

    public void AddAnimMonitor(AnimMixNode node)
    {
        GameObject anim = GameObject.Instantiate(tmp_anim,tmp_anim.transform.parent);
        anim.SetActive(true);
        anims.Add(new DebugAnimItem(anim, node));
    }

    public void ClearItem()
    {
        foreach (var item in anims)
        {
            item.OnDestory();
        }
    }

    public void OnUpdate()
    {
        foreach (var item in anims)
        {
            item.updateData();
        }
    }
}
