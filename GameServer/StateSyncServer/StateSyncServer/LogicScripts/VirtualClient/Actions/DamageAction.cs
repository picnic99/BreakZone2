using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Buffs;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using CharacterManager = StateSyncServer.LogicScripts.Manager.CharacterManager;

namespace StateSyncServer.LogicScripts.VirtualClient.Actions
{
    public class DamageAction : BaseAction
    {
        public override void Apply()
        {
            //最终伤害 == 基础伤害 + 属性加成 + buff加成

            string record = "[{0}]使用[{1}]\n{2}";
            string damageRecord = "";
            if (targets == null || targets.Length <= 0) return;

            //确定一下 施放者最终能造成的伤害 此前伤害只包括 伤害公式计算出的伤害 （基础伤害 + 各种属性加成）
            //此处确定施放者身上施放携带一些增伤的BUFF
            owner.BuffAsk(this);
            owner.eventDispatcher.Event(CharacterEvent.MAKE_DAMAGE, from, value);
            foreach (var target in targets)
            {
                target.BuffAsk(this);
                float finalMakeValue = value.finalValue;
                //此处需要询问步骤来确定最终造成的伤害
                target.property.hp.AddExAddValue(-finalMakeValue);
                target.eventDispatcher.Event(CharacterEvent.PROPERTY_CHANGE);
                {
                    target.eventDispatcher.Event(CharacterEvent.GET_DAMAGE, from, value);
                    damageRecord += " -对[" + target.characterData.characterName + "]造成了<color=#ff0000>[" + finalMakeValue + "]</color>点伤害\n";
                    CharacterManager.GetInstance().Event(CharacterEvent.PROPERTY_CHANGE, PropertyType.HP, value, target);
                }

                string name = "未知";
                if (from is Skill)
                {
                    name = ((Skill)from).skillData.SkillName;
                }
                if (from is Buff)
                {
                    name = ((Buff)from).buffData.buffName;
                }

                record = string.Format(record, owner.characterData.characterName, name, damageRecord);
                RecordManager.GetInstance().AddRecord(record);
            }
        }
    }
}