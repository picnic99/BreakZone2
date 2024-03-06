using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Buffs;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;

namespace StateSyncServer.LogicScripts.VirtualClient.Actions
{
    public abstract class BaseAction
    {
        //行为发出者
        public Character owner;
        //行为接收者
        public Character[] targets;
        //数值
        public PropertyValue value;
        //来源
        public Behaviour from;
        //来源描述
        public string fromStr;

        public void Init(Character character, Character[] targets, float value, Behaviour from)
        {
            owner = character;
            this.targets = targets;
            this.value = new PropertyValue(value);
            this.from = from;
        }
        public abstract void Apply();

        //下次普通攻击伤害提升100% buff
        //下次受到普攻的伤害】、

        //考虑属性 buff

        public bool IsFromSkill()
        {
            return from is Skill;
        }

        public bool IsFromBuff()
        {
            return from is Buff;
        }
    }
}