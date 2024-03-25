using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRoleView
{
    public GameObject tmp_role;

    public List<DebugCharacter> roles; 

    public DebugRoleView(GameObject tmp_role)
    {
        this.tmp_role = tmp_role;
        roles = new List<DebugCharacter>();
    }

    public void AddRole(_Character character)
    {
        GameObject obj = GameObject.Instantiate<GameObject>(tmp_role, tmp_role.transform.parent);
        obj.SetActive(true);
        DebugCharacter item = new DebugCharacter(obj);
        item.UpdateData(character);
        roles.Add(item);
        // if(roles.Count == 1)
        // {
        //     item.Change();
        // }
    }

    public void UpdateData()
    {
        foreach (var item in roles)
        {
            item.UpdateData();
        }
    }

    public void DelRole(DebugCharacter character)
    {
        roles.Remove(character);
        character.Destory();
    }

    public DebugCharacter GetFristRole()
    {
        if (roles.Count <= 0) return null;
        return roles[0];
    }
}
