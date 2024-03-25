using UnityEngine;

public class DataDriverTest : MonoBehaviour
{
    private void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }*/
    }

    public void Init()
    {
        //创建一个数据
        //动画驱动数据
        //特效驱动数据
        var data = new SkillDriverData(2f, 0f, 1f,
            new ActionDriver[] { new ActionDriver(0.8f,0.2f,ActionDriver.ACTION_DAMAGE,100,ActionDriver.RANGE_BOX,new Vector3(2f,1f,2f)) },
            new AnimDriver[] {new AnimDriver(0f, "skill2") },
            new EffectDriver[] { new EffectDriver(0.6f, "Skill/ZhengYi",1f,EffectDriver.POS_TYPE_SOMEPOS,new Vector3(0,0,1.5f),Vector3.zero,new Vector3(0.3f,0.3f,0.3f)) },
            new SoundDriver[] { }
            );


        //实例化一个character
        _Character character = DebugManager.GetInstance().AddRoleReturn();
        character.baseInfo.isDebug = true;

        //实例化一个数据驱动技能
        var skill = new DataDriverSkill();
        skill.character = character;
        skill.driverData = data;
        //把技能传给编辑器 编辑器解析技能

        skill.DoSkill();
    }
}