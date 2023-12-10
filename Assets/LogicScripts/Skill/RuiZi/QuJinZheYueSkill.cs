using System;
using UnityEngine;

public class QuJinZheYueSkill : Skill
{
    public QuJinZheYueSkill()
    {
        skillDurationTime = stateDurationTime = 1f;
    }
    public override void OnEnter()
    {
        PlayAnim("atk3");
        //创建两个特效立场
        //一个为进入一个为出去
        //位置位于角色前方1m处 出口位于前方10m处 所有角色可从两面穿越来回 最多持续5s
        base.OnEnter();
    }

    public override void OnTrigger()
    {
        new QuJingZheYue(character);
    }

    class QuJingZheYue
    {
        Character character;
        GameObject skillInstance;
        Transform inObj;
        Transform outObj;
        string path = "Skill/QuJingZheYue";
        float skillDurationTime = 5f;
        public QuJingZheYue(Character character)
        {
            this.character = character;
            skillInstance = ResourceManager.GetInstance().GetObjInstance<GameObject>(path);
            this.InitTransform();
            this.AddBehaviour();
        }

        private void InitTransform()
        {
            inObj = skillInstance.transform.Find("in");
            outObj = skillInstance.transform.Find("out");
            skillInstance.transform.forward = character.trans.forward;
            skillInstance.transform.position = character.trans.position + character.trans.forward;
            outObj.transform.localPosition = new Vector3(0, 1, 10); 
        }

        private void AddBehaviour()
        {
            inObj.GetComponent<ColliderHelper>().OnTriggerEnterCall += Transport;
            TimeManager.GetInstance().AddOnceTimer(this, skillDurationTime, () => {
                GameObject.Destroy(skillInstance);
            });
        }

        private void Transport(Collider col)
        {
            col.transform.position = new Vector3(outObj.transform.position.x, col.transform.position.y, outObj.transform.position.z);
        }
    }

}
