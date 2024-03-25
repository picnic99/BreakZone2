using UnityEngine;

public class DataDriverSkill
{
    public _Character character;
    public SkillDriverData driverData;

    public void DoSkill()
    {
        character.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, StateType.DoSkill, 4);
        int maxLen =
            Mathf.Max(
            Mathf.Max(
                driverData.animDriver.Length, 
                driverData.effectDriver.Length), 
            driverData.actionDrivers.Length);


        for (int i = 0; i < maxLen; i++)
        {
            if(i < driverData.animDriver.Length)
            {
                AddAnim(driverData.animDriver[i]);
            }
            if (i < driverData.effectDriver.Length)
            {
                AddEffect(driverData.effectDriver[i]);
            }
            if (i < driverData.actionDrivers.Length)
            {
                AddAction(driverData.actionDrivers[i]);
            }
        }
    }

    /// <summary>
    /// 添加一个动画
    /// </summary>
    /// <param name="inTime">在什么时间点</param>
    /// <param name="animName">动画名称叫什么</param>
    private void AddAnim(AnimDriver anim)
    {
        TimeManager.GetInstance().AddOnceTimer(this, anim.inTime, ()=> { 
            AnimManager.GetInstance().PlayAnim(character, anim.animName);
        });
    }

    /// <summary>
    /// 添加一个特效
    /// </summary>
    /// <param name="effect"></param>
    private void AddEffect(EffectDriver effect)
    {
        Transform parent = null;
        if(effect.posType == EffectDriver.POS_TYPE_FOLLOW)
        {
            parent = character.trans;
        }

        TimeManager.GetInstance().AddOnceTimer(this, effect.inTime, () => {
            EffectManager.GetInstance().PlayEffect(
                effect.effectName,
                effect.drationTime,
                parent, 
                effect.pos, 
                effect.rotate,
                effect.scale
                );
        });
    }

    /// <summary>
    /// 添加一个BUFF
    /// </summary>
    /// <param name="buff"></param>
    private void AddBuff(BuffDriver buff)
    {

    }

    private void AddAction(ActionDriver action)
    {
        TimeManager.GetInstance().AddOnceTimer(this, action.inTime, () => {
            Collider[] cols = Physics.OverlapBox(character.trans.position,action.rangeParam/2);
            GameObject obj = GameObject.Instantiate(ResourceManager.GetInstance().LoadResource<GameObject>("RangeShow/boxRange"));
            obj.transform.localScale = action.rangeParam;
            obj.transform.position = character.trans.position;
            foreach (var item in cols)
            {
                Debug.Log("本次触发："+ item.name);
            }
            ActionManager.GetInstance().AddDamageAction(character, new _Character[] { }, action.inTime, null);
            TimeManager.GetInstance().AddOnceTimer(this, action.duration, () =>
            {
                GameObject.Destroy(obj);
            });
        });
    }

    private void AddSound(SoundDriver sound)
    {

    }
}