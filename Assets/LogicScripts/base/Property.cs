
using UnityEngine;

public enum PropertyType
{
    HP, SHIELD, ATK, DEFEND, PCTDEFEND, ATKSPEED, MOVESPEED
}

/// <summary>
/// 角色属性
/// 还有很多数值要添加
/// </summary>
public class Property
{
    private _Character character;
    //生命值
    public PropertyValue hp = new PropertyValue(1000);
    //护盾
    public PropertyValue shield = new PropertyValue(0);
    //攻击力
    public PropertyValue atk = new PropertyValue(100);
    //防御力
    public PropertyValue defend = new PropertyValue(10);//固定防御 如值为10 伤害为 100 则固定格挡10点伤害 
    //百分比防御 免伤
    public PropertyValue pctDefend = new PropertyValue(0);//百分防御 如值为0.3 伤害为100 则可防御30点伤害 
    //攻击速度
    public PropertyValue atkSpeed = new PropertyValue(1);
    //移动速度
    public PropertyValue moveSpeed = new PropertyValue(1);

    public Property(_Character character)
    {
        this.character = character;
        initProperty();
    }

    private void initProperty()
    {
        var vo = CharacterConfiger.GetInstance().GetPropertyById(character.characterData.id);
        if (vo == null) return;
        hp = new PropertyValue(vo.property.Hp);
        shield = new PropertyValue(vo.property.Defend);
        atk = new PropertyValue(vo.property.Atk);
        defend = new PropertyValue(vo.property.Defend);
        pctDefend = new PropertyValue(vo.property.Defend);
        atkSpeed = new PropertyValue(vo.property.AtkSpeed);
        moveSpeed = new PropertyValue(vo.property.MoveSpeed);
    }

    public float Hp { get { return atk.finalValue; } private set { } }
    public float Shield { get { return shield.finalValue; } private set { } }
    public float Atk { get { return atk.finalValue; } private set { } }
    public float Defend { get { return defend.finalValue; } private set { } }
    public float PctDefend { get { return pctDefend.finalValue; } private set { } }
    public float AtkSpeed { get { return atkSpeed.finalValue; } private set { } }
    public float MoveSpeed { get { return moveSpeed.finalValue; } private set { } }

    public ValueModifier<float> AddValue(PropertyBuffVO vo)
    {
        ValueModifier<float> mod = null;
        PropertyValue pv = null;
        switch (vo.propertyType)
        {
            case PropertyType.HP:
                mod = vo.isPct ? hp.AddExPctAddValue(vo.value) : hp.AddExAddValue(vo.value);
                pv = hp;
                break;
            case PropertyType.SHIELD:
                mod = vo.isPct ? shield.AddExPctAddValue(vo.value) : shield.AddExAddValue(vo.value);
                pv = shield;
                break;
            case PropertyType.ATK:
                mod = vo.isPct ? atk.AddExPctAddValue(vo.value) : atk.AddExAddValue(vo.value);
                pv = atk;
                break;
            case PropertyType.DEFEND:
                mod = vo.isPct ? defend.AddExPctAddValue(vo.value) : defend.AddExAddValue(vo.value);
                pv = defend;
                break;
            case PropertyType.PCTDEFEND:
                mod = vo.isPct ? pctDefend.AddExPctAddValue(vo.value) : pctDefend.AddExAddValue(vo.value);
                pv = pctDefend;
                break;
            case PropertyType.ATKSPEED:
                mod = vo.isPct ? atkSpeed.AddExPctAddValue(vo.value) : atkSpeed.AddExAddValue(vo.value);
                pv = atkSpeed;
                break;
            case PropertyType.MOVESPEED:
                mod = vo.isPct ? moveSpeed.AddExPctAddValue(vo.value) : moveSpeed.AddExAddValue(vo.value);
                pv = moveSpeed;
                break;
            default:
                break;
        }
        if (mod != null)
        {
            character.eventDispatcher.Event(CharacterEvent.PROPERTY_CHANGE, vo.propertyType,pv,character);
            Debug.Log(character.characterData.characterName + " PROPERTY_CHANGE");
        }
        return mod;
    }

    public void RemoveValue(PropertyBuffVO vo, ValueModifier<float> mod)
    {
        switch (vo.propertyType)
        {
            case PropertyType.HP:
                if (vo.isPct) hp.RemoveExPctAddValue(mod);
                else hp.RemoveExAddValue(mod);
                break;
            case PropertyType.SHIELD:
                if (vo.isPct) shield.RemoveExPctAddValue(mod);
                else shield.RemoveExAddValue(mod);
                break;
            case PropertyType.ATK:
                if (vo.isPct) atk.RemoveExPctAddValue(mod);
                else atk.RemoveExAddValue(mod);
                break;
            case PropertyType.DEFEND:
                if (vo.isPct) defend.RemoveExPctAddValue(mod);
                else defend.RemoveExAddValue(mod);
                break;
            case PropertyType.PCTDEFEND:
                if (vo.isPct) pctDefend.RemoveExPctAddValue(mod);
                else pctDefend.RemoveExAddValue(mod);
                break;
            case PropertyType.ATKSPEED:
                if (vo.isPct) atkSpeed.RemoveExPctAddValue(mod);
                else atkSpeed.RemoveExAddValue(mod);
                break;
            case PropertyType.MOVESPEED:
                if (vo.isPct) moveSpeed.RemoveExPctAddValue(mod);
                else moveSpeed.RemoveExAddValue(mod);
                break;
            default:
                break;
        }
        character.eventDispatcher.Event(CharacterEvent.PROPERTY_CHANGE, vo.propertyType);
    }

    public PropertyValue GetPropertyByType(PropertyType type)
    {
        PropertyValue value = null;
        switch (type)
        {
            case PropertyType.HP:value = hp;
                break;
            case PropertyType.SHIELD:value = shield;
                break;
            case PropertyType.ATK:value = atk;
                break;
            case PropertyType.DEFEND:value = defend;
                break;
            case PropertyType.PCTDEFEND:value = pctDefend;
                break;
            case PropertyType.ATKSPEED:value = atkSpeed;
                break;
            case PropertyType.MOVESPEED:value = moveSpeed;
                break;
            default:
                break;
        }
        return value;
    }

    public bool IsDie()
    {
        return hp.finalValue <= 0;
    }


    public string GetDesc()
    {
        return "\n" +
            "生命值 " + hp.finalValue + "/" + hp.baseValue + "\n" +
            "护盾 " + shield.finalValue + " " +
            "攻击力 " + atk.finalValue + " " +
            "防御 " + defend.finalValue + "\n" +
            "伤害减免 " + pctDefend.finalValue + "\n" +
            "攻击速度 " + atkSpeed.finalValue + " " +
            "移动速度 " + moveSpeed.finalValue + "\n"
            ;
    }
}