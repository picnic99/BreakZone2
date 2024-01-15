using UnityEngine;

public class SkillInstance2
{
    public Skill skill;
    public GameObject root;

    public SkillInstance2()
    {

    }


    public void Init(Skill skill,string name, Vector3 createPos, Vector3 moveTo, float moveSpeed)
    {
        this.skill = skill;
        if (root != null) GameObject.Destroy(root);
        //实例化技能
        GameObject tmp_obj = Resources.Load<GameObject>(name);
        root = GameObject.Instantiate<GameObject>(tmp_obj);
        //设置各类参数
    }

    public void OnUpdate()
    {
        //检测是否碰到了目标 碰到后需要返回目标给skill skill进行后续处理
        //RangeScan.CheckShphere();
    }
}