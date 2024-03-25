using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTargetView
{
    public GameObject tmp_target;
    public List<DebugTargetItem> targets;

    public DebugTargetView(GameObject tmp_target)
    {
        this.tmp_target = tmp_target;
        targets = new List<DebugTargetItem>();
    }

    public void AddTarget(_Character character)
    {
        GameObject obj = GameObject.Instantiate<GameObject>(tmp_target, tmp_target.transform.parent);
        obj.SetActive(true);
        var item = new DebugTargetItem(obj, character);
        targets.Add(item);
    }

    public void RemoveTarget(DebugTargetItem item)
    {
        targets.Remove(item);
    }

    public DebugTargetItem GetTarget(_Character character)
    {
        return targets.Find((DebugTargetItem item) =>
        {
            return item.character == character;
        });
    }

    /// <summary>
    /// ��ȡ����Ŀ��
    /// </summary>
    /// <returns></returns>
    public List<_Character> GetTargets()
    {
        List<_Character> result = new List<_Character>();
        foreach (var item in targets)
        {
            result.Add(item.character);
        }
        return result;
    }
}
