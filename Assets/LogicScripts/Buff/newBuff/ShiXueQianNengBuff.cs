using System;
using UnityEngine;
/// <summary>
/// 嗜血潜能 血量低于50%时 每减少1%的血提升5点攻击力和1%的移动速度 最大提升200点攻击力和40%的移动速度
/// </summary>
public class ShiXueQianNengBuff : BuffGroup
{
    public ShiXueQianNengBuff()
    {
        buffData = new BuffVO("嗜血潜能");
    }
    public override void Init()
    {
        base.Init();
        durationTime = 15;
        TriggerBuffComponent buff = AddTriggerBuff();
        buff.AddTrigger_PROPERTY_CHANGE(character, PropertyType.HP, MathType.LESS_OR_EQUAL, 0.5f);
        ValueModifier<float> atkMod = null;
        ValueModifier<float> moveMod = null;
        Action<object[]> fun = null;
        buff.doBuffCall = (args) =>
        {
            durationTime = 10;
            buff.AddEnd_DURATION_TIME(10);
            var hp = character.property.hp;
            float oldHpRate = hp.finalValue / hp.baseValue;
            fun = (args) =>
            {
                float newHpRate = hp.finalValue / hp.baseValue;
                int gapRate = (int)Mathf.Floor(Math.Max((oldHpRate - newHpRate)*100, 0));
                var atk = character.property.atk;
                var moveSpeed = character.property.moveSpeed;
                if (atkMod == null)
                    atkMod = atk.AddExAddValue(Math.Min(5 * gapRate, 200));
                else
                    atk.ModModifier(atkMod, Math.Min(5 * gapRate, 200));

                if (moveMod == null)
                    moveMod = moveSpeed.AddExPctAddValue(Math.Min(0.01f * gapRate, 0.4f));
                else
                    moveSpeed.ModModifier(moveMod, Math.Min(0.01f * gapRate, 0.4f));

            };
            character.eventDispatcher.On(CharacterEvent.PROPERTY_CHANGE, fun);
        };

        buff.endCall = () =>
        {
            character.property.atk.RemoveExAddValue(atkMod);
            character.property.moveSpeed.RemoveExPctAddValue(moveMod);
            character.eventDispatcher.Off(CharacterEvent.PROPERTY_CHANGE, fun);
        };

        buff.AddEnd_DURATION_TIME(durationTime);
        AddBuffComponent(buff);
    }
    public override BuffFlag GetFlag()
    {
        return BuffFlag.POSITIVE;
    }
    public override string GetDesc()
    {
        if (buffComponents != null && buffComponents.Count >= 1)
        {
            return "[嗜血潜能]:血量低于50%时 每减少1%的血提升5点攻击力和1%的移动速度 最大提升200点攻击力和40%的移动速度  持续时间" + buffComponents[0].durationTime.ToString("F2") + "s";
        }
        return "";
    }
}